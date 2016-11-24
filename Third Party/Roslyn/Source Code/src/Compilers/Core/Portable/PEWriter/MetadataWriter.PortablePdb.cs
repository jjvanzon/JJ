﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using Microsoft.CodeAnalysis;
using Roslyn.Utilities;

namespace Microsoft.Cci
{
    using Roslyn.Reflection;
    using Roslyn.Reflection.Metadata.Ecma335;
    using Roslyn.Reflection.Metadata.Ecma335.Blobs;
    
    internal partial class MetadataWriter
    {
        /// <summary>
        /// Import scopes are associated with binders (in C#) and thus multiple instances might be created for a single set of imports.
        /// We consider scopes with the same parent and the same imports the same.
        /// Internal for testing.
        /// </summary>
        internal sealed class ImportScopeEqualityComparer : IEqualityComparer<IImportScope>
        {
            public static readonly ImportScopeEqualityComparer Instance = new ImportScopeEqualityComparer();

            public bool Equals(IImportScope x, IImportScope y)
            {
                return (object)x == y ||
                       x != null && y != null && Equals(x.Parent, y.Parent) && x.GetUsedNamespaces().SequenceEqual(y.GetUsedNamespaces());
            }

            public int GetHashCode(IImportScope obj)
            {
                return Hash.Combine(Hash.CombineValues(obj.GetUsedNamespaces()), obj.Parent != null ? GetHashCode(obj.Parent) : 0);
            }
        }

        private readonly Dictionary<DebugSourceDocument, DocumentHandle> _documentIndex = new Dictionary<DebugSourceDocument, DocumentHandle>();
        private readonly Dictionary<IImportScope, ImportScopeHandle> _scopeIndex = new Dictionary<IImportScope, ImportScopeHandle>(ImportScopeEqualityComparer.Instance);

        private void SerializeMethodDebugInfo(IMethodBody bodyOpt, int methodRid, StandaloneSignatureHandle localSignatureHandleOpt, ref LocalVariableHandle lastLocalVariableHandle, ref LocalConstantHandle lastLocalConstantHandle)
        {
            if (bodyOpt == null)
            {
                _debugMetadataOpt.AddMethodDebugInformation(default(DocumentHandle), default(BlobHandle));
                return;
            }

            bool isIterator = bodyOpt.StateMachineTypeName != null;
            bool emitDebugInfo = isIterator || bodyOpt.HasAnySequencePoints;

            if (!emitDebugInfo)
            {
                _debugMetadataOpt.AddMethodDebugInformation(default(DocumentHandle), default(BlobHandle));
                return;
            }

            var methodHandle = MetadataTokens.MethodDefinitionHandle(methodRid);

            var bodyImportScope = bodyOpt.ImportScope;
            var importScopeHandle = (bodyImportScope != null) ? GetImportScopeIndex(bodyImportScope, _scopeIndex) : default(ImportScopeHandle);

            // documents & sequence points:
            DocumentHandle singleDocumentHandle;
            ArrayBuilder<Cci.SequencePoint> sequencePoints = ArrayBuilder<Cci.SequencePoint>.GetInstance();
            bodyOpt.GetSequencePoints(sequencePoints);
            BlobHandle sequencePointsBlob = SerializeSequencePoints(localSignatureHandleOpt, sequencePoints.ToImmutableAndFree(), _documentIndex, out singleDocumentHandle);

            _debugMetadataOpt.AddMethodDebugInformation(document: singleDocumentHandle, sequencePoints: sequencePointsBlob);

            // Unlike native PDB we don't emit an empty root scope.
            // scopes are already ordered by StartOffset ascending then by EndOffset descending (the longest scope first).

            if (bodyOpt.LocalScopes.Length == 0)
            {
                // TODO: the compiler should produce a scope for each debuggable method 
                _debugMetadataOpt.AddLocalScope(
                    method: methodHandle,
                    importScope: importScopeHandle,
                    variableList: NextHandle(lastLocalVariableHandle),
                    constantList: NextHandle(lastLocalConstantHandle),
                    startOffset: 0,
                    length: bodyOpt.IL.Length);
            }
            else
            {
                foreach (LocalScope scope in bodyOpt.LocalScopes)
                {
                    _debugMetadataOpt.AddLocalScope(
                        method: methodHandle,
                        importScope: importScopeHandle,
                        variableList: NextHandle(lastLocalVariableHandle),
                        constantList: NextHandle(lastLocalConstantHandle),
                        startOffset: scope.StartOffset,
                        length: scope.Length);

                    foreach (ILocalDefinition local in scope.Variables)
                    {
                        Debug.Assert(local.SlotIndex >= 0);

                        lastLocalVariableHandle = _debugMetadataOpt.AddLocalVariable(
                            attributes: local.PdbAttributes,
                            index: local.SlotIndex,
                            name: _debugMetadataOpt.GetOrAddString(local.Name));

                        SerializeDynamicLocalInfo(local, lastLocalVariableHandle);
                    }

                    foreach (ILocalDefinition constant in scope.Constants)
                    {
                        var mdConstant = constant.CompileTimeValue;
                        Debug.Assert(mdConstant != null);

                        lastLocalConstantHandle = _debugMetadataOpt.AddLocalConstant(
                            name: _debugMetadataOpt.GetOrAddString(constant.Name),
                            signature: SerializeLocalConstantSignature(constant));

                        SerializeDynamicLocalInfo(constant, lastLocalConstantHandle);
                    }
                }
            }

            var asyncDebugInfo = bodyOpt.AsyncDebugInfo;
            if (asyncDebugInfo != null)
            {
                _debugMetadataOpt.AddStateMachineMethod(
                    moveNextMethod: methodHandle,
                    kickoffMethod: GetMethodDefinitionHandle(asyncDebugInfo.KickoffMethod));

                SerializeAsyncMethodSteppingInfo(asyncDebugInfo, methodHandle);
            }

            SerializeStateMachineLocalScopes(bodyOpt, methodHandle);

            // delta doesn't need this information - we use information recorded by previous generation emit
            if (Context.ModuleBuilder.CommonCompilation.Options.EnableEditAndContinue && !IsFullMetadata)
            {
                SerializeEncMethodDebugInformation(bodyOpt, methodHandle);
            }
        }

        private static LocalVariableHandle NextHandle(LocalVariableHandle handle) => 
            MetadataTokens.LocalVariableHandle(MetadataTokens.GetRowNumber(handle) + 1);

        private static LocalConstantHandle NextHandle(LocalConstantHandle handle) => 
            MetadataTokens.LocalConstantHandle(MetadataTokens.GetRowNumber(handle) + 1);

        private BlobHandle SerializeLocalConstantSignature(ILocalDefinition localConstant)
        {
            var builder = new BlobBuilder();

            // TODO: BlobEncoder.LocalConstantSignature

            // CustomMod*
            var encoder = new CustomModifiersEncoder(builder);
            SerializeCustomModifiers(encoder, localConstant.CustomModifiers);

            var type = localConstant.Type;
            var typeCode = type.TypeCode(Context);

            object value = localConstant.CompileTimeValue.Value;

            // PrimitiveConstant or EnumConstant
            if (value is decimal)
            {
                builder.WriteByte(0x11);
                builder.WriteCompressedInteger(CodedIndex.ToTypeDefOrRefOrSpec(GetTypeHandle(type)));

                builder.WriteDecimal((decimal)value);
            }
            else if (value is DateTime)
            {
                builder.WriteByte(0x11);
                builder.WriteCompressedInteger(CodedIndex.ToTypeDefOrRefOrSpec(GetTypeHandle(type)));

                builder.WriteDateTime((DateTime)value);
            }
            else if (typeCode == PrimitiveTypeCode.String)
            {
                builder.WriteByte((byte)ConstantTypeCode.String);
                if (value == null)
                {
                    builder.WriteByte(0xff);
                }
                else
                {
                    builder.WriteUTF16((string)value);
                }
            }
            else if (value != null)
            {
                // TypeCode
                builder.WriteByte((byte)MetadataWriterUtilities.GetConstantTypeCode(value));


                // Value
                builder.WriteConstant(value);

                // EnumType
                if (type.IsEnum)
                {
                    builder.WriteCompressedInteger(CodedIndex.ToTypeDefOrRefOrSpec(GetTypeHandle(type)));
                }
            }
            else if (this.module.IsPlatformType(type, PlatformType.SystemObject))
            {
                builder.WriteByte(0x1c);
            }
            else
            {
                builder.WriteByte((byte)(type.IsValueType ? 0x11 : 0x12));
                builder.WriteCompressedInteger(CodedIndex.ToTypeDefOrRefOrSpec(GetTypeHandle(type)));
            }

            return _debugMetadataOpt.GetOrAddBlob(builder);
        }

        #region ImportScope

        private static readonly ImportScopeHandle ModuleImportScopeHandle = MetadataTokens.ImportScopeHandle(1);

        private void SerializeImport(BlobBuilder writer, AssemblyReferenceAlias alias)
        {
            // <import> ::= AliasAssemblyReference <alias> <target-assembly>
            writer.WriteByte((byte)ImportDefinitionKind.AliasAssemblyReference);
            writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(alias.Name)));
            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(GetOrAddAssemblyReferenceHandle(alias.Assembly)));
        }

        private void SerializeImport(BlobBuilder writer, UsedNamespaceOrType import)
        {
            if (import.TargetXmlNamespaceOpt != null)
            {
                Debug.Assert(import.TargetNamespaceOpt == null);
                Debug.Assert(import.TargetAssemblyOpt == null);
                Debug.Assert(import.TargetTypeOpt == null);

                // <import> ::= ImportXmlNamespace <alias> <target-namespace>
                writer.WriteByte((byte)ImportDefinitionKind.ImportXmlNamespace);
                writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(import.AliasOpt)));
                writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(import.TargetXmlNamespaceOpt)));
            }
            else if (import.TargetTypeOpt != null)
            {
                Debug.Assert(import.TargetNamespaceOpt == null);
                Debug.Assert(import.TargetAssemblyOpt == null);

                if (import.AliasOpt != null)
                {
                    // <import> ::= AliasType <alias> <target-type>
                    writer.WriteByte((byte)ImportDefinitionKind.AliasType);
                    writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(import.AliasOpt)));
                }
                else
                {
                    // <import> ::= ImportType <target-type>
                    writer.WriteByte((byte)ImportDefinitionKind.ImportType);
                }

                writer.WriteCompressedInteger(CodedIndex.ToTypeDefOrRefOrSpec(GetTypeHandle(import.TargetTypeOpt))); // TODO: index in release build
            }
            else if (import.TargetNamespaceOpt != null)
            {
                if (import.TargetAssemblyOpt != null)
                {
                    if (import.AliasOpt != null)
                    {
                        // <import> ::= AliasAssemblyNamespace <alias> <target-assembly> <target-namespace>
                        writer.WriteByte((byte)ImportDefinitionKind.AliasAssemblyNamespace);
                        writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(import.AliasOpt)));
                    }
                    else
                    {
                        // <import> ::= ImportAssemblyNamespace <target-assembly> <target-namespace>
                        writer.WriteByte((byte)ImportDefinitionKind.ImportAssemblyNamespace);
                    }

                    writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(GetAssemblyReferenceHandle(import.TargetAssemblyOpt)));
                }
                else
                {
                    if (import.AliasOpt != null)
                    {
                        // <import> ::= AliasNamespace <alias> <target-namespace>
                        writer.WriteByte((byte)ImportDefinitionKind.AliasNamespace);
                        writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(import.AliasOpt)));
                    }
                    else
                    {
                        // <import> ::= ImportNamespace <target-namespace>
                        writer.WriteByte((byte)ImportDefinitionKind.ImportNamespace);
                    }
                }

                // TODO: cache?
                string namespaceName = TypeNameSerializer.BuildQualifiedNamespaceName(import.TargetNamespaceOpt);
                writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(namespaceName)));
            }
            else
            {
                // <import> ::= ImportReferenceAlias <alias>
                Debug.Assert(import.AliasOpt != null);
                Debug.Assert(import.TargetAssemblyOpt == null);

                writer.WriteByte((byte)ImportDefinitionKind.ImportAssemblyReferenceAlias);
                writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(_debugMetadataOpt.GetOrAddBlobUtf8(import.AliasOpt)));
            }
        }

        private void DefineModuleImportScope()
        {
            // module-level import scope:
            var writer = new BlobBuilder();

            SerializeModuleDefaultNamespace();

            foreach (AssemblyReferenceAlias alias in module.GetAssemblyReferenceAliases(Context))
            {
                SerializeImport(writer, alias);
            }

            foreach (UsedNamespaceOrType import in module.GetImports())
            {
                SerializeImport(writer, import);
            }

            var rid = _debugMetadataOpt.AddImportScope(
                parentScope: default(ImportScopeHandle),
                imports: _debugMetadataOpt.GetOrAddBlob(writer));

            Debug.Assert(rid == ModuleImportScopeHandle);
        }

        private ImportScopeHandle GetImportScopeIndex(IImportScope scope, Dictionary<IImportScope, ImportScopeHandle> scopeIndex)
        {
            ImportScopeHandle scopeHandle;
            if (scopeIndex.TryGetValue(scope, out scopeHandle))
            {
                // scope is already indexed:
                return scopeHandle;
            }

            var parent = scope.Parent;
            var parentScopeHandle = (parent != null) ? GetImportScopeIndex(scope.Parent, scopeIndex) : ModuleImportScopeHandle;

            var result = _debugMetadataOpt.AddImportScope(
                parentScope: parentScopeHandle,
                imports: SerializeImportsBlob(scope));

            scopeIndex.Add(scope, result);
            return result;
        }

        private BlobHandle SerializeImportsBlob(IImportScope scope)
        {
            var writer = new BlobBuilder();

            foreach (UsedNamespaceOrType import in scope.GetUsedNamespaces())
            {
                SerializeImport(writer, import);
            }

            return _debugMetadataOpt.GetOrAddBlob(writer);
        }

        private void SerializeModuleDefaultNamespace()
        {
            // C#: DefaultNamespace is null.
            // VB: DefaultNamespace is non-null.
            if (module.DefaultNamespace == null)
            {
                return;
            }

            _debugMetadataOpt.AddCustomDebugInformation(
                parent: EntityHandle.ModuleDefinition,
                kind: _debugMetadataOpt.GetOrAddGuid(PortableCustomDebugInfoKinds.DefaultNamespace),
                value: _debugMetadataOpt.GetOrAddBlobUtf8(module.DefaultNamespace));
        }

        #endregion

        #region Locals

        private void SerializeDynamicLocalInfo(ILocalDefinition local, EntityHandle parent)
        {
            var dynamicFlags = local.DynamicTransformFlags;
            if (dynamicFlags.IsDefault)
            {
                return;
            }

            var value = SerializeBitVector(dynamicFlags);

            _debugMetadataOpt.AddCustomDebugInformation(
                parent: parent,
                kind: _debugMetadataOpt.GetOrAddGuid(PortableCustomDebugInfoKinds.DynamicLocalVariables),
                value: _debugMetadataOpt.GetOrAddBlob(value));
        }

        private static ImmutableArray<byte> SerializeBitVector(ImmutableArray<TypedConstant> vector)
        {
            var builder = ArrayBuilder<byte>.GetInstance();

            int b = 0;
            int shift = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                if ((bool)vector[i].Value)
                {
                    b |= 1 << shift;
                }

                if (shift == 7)
                {
                    builder.Add((byte)b);
                    b = 0;
                    shift = 0;
                }
                else
                {
                    shift++;
                }
            }

            if (b != 0)
            {
                builder.Add((byte)b);
            }
            else
            {
                // trim trailing zeros:
                int lastNonZero = builder.Count - 1;
                while (builder[lastNonZero] == 0)
                {
                    lastNonZero--;
                }

                builder.Clip(lastNonZero + 1);
            }

            return builder.ToImmutableAndFree();
        }

        #endregion

        #region State Machines

        private void SerializeAsyncMethodSteppingInfo(AsyncMethodBodyDebugInfo asyncInfo, MethodDefinitionHandle moveNextMethod)
        {
            Debug.Assert(asyncInfo.ResumeOffsets.Length == asyncInfo.YieldOffsets.Length);
            Debug.Assert(asyncInfo.CatchHandlerOffset >= -1);

            var writer = new BlobBuilder();

            writer.WriteUInt32((uint)((long)asyncInfo.CatchHandlerOffset + 1));

            for (int i = 0; i < asyncInfo.ResumeOffsets.Length; i++)
            {
                writer.WriteUInt32((uint)asyncInfo.YieldOffsets[i]);
                writer.WriteUInt32((uint)asyncInfo.ResumeOffsets[i]);
                writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(moveNextMethod));
            }

            _debugMetadataOpt.AddCustomDebugInformation(
                parent: moveNextMethod,
                kind: _debugMetadataOpt.GetOrAddGuid(PortableCustomDebugInfoKinds.AsyncMethodSteppingInformationBlob),
                value: _debugMetadataOpt.GetOrAddBlob(writer));
        }

        private void SerializeStateMachineLocalScopes(IMethodBody methodBody, MethodDefinitionHandle method)
        {
            var scopes = methodBody.StateMachineHoistedLocalScopes;
            if (scopes.IsDefaultOrEmpty)
            {
                return;
            }

            var writer = new BlobBuilder();

            foreach (var scope in scopes)
            {
                writer.WriteUInt32((uint)scope.StartOffset);
                writer.WriteUInt32((uint)scope.Length);
            }

            _debugMetadataOpt.AddCustomDebugInformation(
                parent: method,
                kind: _debugMetadataOpt.GetOrAddGuid(PortableCustomDebugInfoKinds.StateMachineHoistedLocalScopes),
                value: _debugMetadataOpt.GetOrAddBlob(writer));
        }

        #endregion

        #region Sequence Points

        private BlobHandle SerializeSequencePoints(
            StandaloneSignatureHandle localSignatureHandleOpt,
            ImmutableArray<SequencePoint> sequencePoints,
            Dictionary<DebugSourceDocument, DocumentHandle> documentIndex,
            out DocumentHandle singleDocumentHandle)
        {
            if (sequencePoints.Length == 0)
            {
                singleDocumentHandle = default(DocumentHandle);
                return default(BlobHandle);
            }

            var writer = new BlobBuilder();

            int previousNonHiddenStartLine = -1;
            int previousNonHiddenStartColumn = -1;

            // header:
            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(localSignatureHandleOpt));

            var previousDocument = TryGetSingleDocument(sequencePoints);
            singleDocumentHandle = (previousDocument != null) ? GetOrAddDocument(previousDocument, documentIndex) : default(DocumentHandle);

            for (int i = 0; i < sequencePoints.Length; i++)
            {
                var currentDocument = sequencePoints[i].Document;
                if (previousDocument != currentDocument)
                {
                    var documentHandle = GetOrAddDocument(currentDocument, documentIndex);

                    // optional document in header or document record:
                    if (previousDocument != null)
                    {
                        writer.WriteCompressedInteger(0);
                    }

                    writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(documentHandle));
                    previousDocument = currentDocument;
                }

                // delta IL offset:
                if (i > 0)
                {
                    writer.WriteCompressedInteger(sequencePoints[i].Offset - sequencePoints[i - 1].Offset);
                }
                else
                {
                    writer.WriteCompressedInteger(sequencePoints[i].Offset);
                }

                if (sequencePoints[i].IsHidden)
                {
                    writer.WriteInt16(0);
                    continue;
                }

                // Delta Lines & Columns:
                SerializeDeltaLinesAndColumns(writer, sequencePoints[i]);

                // delta Start Lines & Columns:
                if (previousNonHiddenStartLine < 0)
                {
                    Debug.Assert(previousNonHiddenStartColumn < 0);
                    writer.WriteCompressedInteger(sequencePoints[i].StartLine);
                    writer.WriteCompressedInteger(sequencePoints[i].StartColumn);
                }
                else
                {
                    writer.WriteCompressedSignedInteger(sequencePoints[i].StartLine - previousNonHiddenStartLine);
                    writer.WriteCompressedSignedInteger(sequencePoints[i].StartColumn - previousNonHiddenStartColumn);
                }

                previousNonHiddenStartLine = sequencePoints[i].StartLine;
                previousNonHiddenStartColumn = sequencePoints[i].StartColumn;
            }

            return _debugMetadataOpt.GetOrAddBlob(writer);
        }

        private static DebugSourceDocument TryGetSingleDocument(ImmutableArray<SequencePoint> sequencePoints)
        {
            DebugSourceDocument singleDocument = sequencePoints[0].Document;
            for (int i = 1; i < sequencePoints.Length; i++)
            {
                if (sequencePoints[i].Document != singleDocument)
                {
                    return null;
                }
            }

            return singleDocument;
        }

        private void SerializeDeltaLinesAndColumns(BlobBuilder writer, SequencePoint sequencePoint)
        {
            int deltaLines = sequencePoint.EndLine - sequencePoint.StartLine;
            int deltaColumns = sequencePoint.EndColumn - sequencePoint.StartColumn;

            // only hidden sequence points have zero width
            Debug.Assert(deltaLines != 0 || deltaColumns != 0 || sequencePoint.IsHidden);

            writer.WriteCompressedInteger(deltaLines);

            if (deltaLines == 0)
            {
                writer.WriteCompressedInteger(deltaColumns);
            }
            else
            {
                writer.WriteCompressedSignedInteger(deltaColumns);
            }
        }

        #endregion

        #region Documents

        private DocumentHandle GetOrAddDocument(DebugSourceDocument document, Dictionary<DebugSourceDocument, DocumentHandle> index)
        {
            DocumentHandle documentHandle;
            if (!index.TryGetValue(document, out documentHandle))
            {
                var checksumAndAlgorithm = document.ChecksumAndAlgorithm;

                documentHandle = _debugMetadataOpt.AddDocument(
                    name: SerializeDocumentName(document.Location),
                    hashAlgorithm: checksumAndAlgorithm.Item1.IsDefault ? default(GuidHandle) : _debugMetadataOpt.GetOrAddGuid(checksumAndAlgorithm.Item2),
                    hash: (checksumAndAlgorithm.Item1.IsDefault) ? default(BlobHandle) : _debugMetadataOpt.GetOrAddBlob(checksumAndAlgorithm.Item1),
                    language: _debugMetadataOpt.GetOrAddGuid(document.Language));

                index.Add(document, documentHandle);
            }

            return documentHandle;
        }

        private static readonly char[] s_separator1 = { '/' };
        private static readonly char[] s_separator2 = { '\\' };

        private BlobHandle SerializeDocumentName(string name)
        {
            Debug.Assert(name != null);

            var writer = new BlobBuilder();

            int c1 = Count(name, s_separator1[0]);
            int c2 = Count(name, s_separator2[0]);
            char[] separator = (c1 >= c2) ? s_separator1 : s_separator2;

            writer.WriteByte((byte)separator[0]);

            // TODO: avoid allocations
            foreach (var part in name.Split(separator))
            {
                BlobHandle partIndex = _debugMetadataOpt.GetOrAddBlob(ImmutableArray.Create(s_utf8Encoding.GetBytes(part)));
                writer.WriteCompressedInteger(_debugMetadataOpt.GetHeapOffset(partIndex));
            }

            return _debugMetadataOpt.GetOrAddBlob(writer);
        }

        private static int Count(string str, char c)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == c)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion

        #region Edit and Continue

        private void SerializeEncMethodDebugInformation(IMethodBody methodBody, MethodDefinitionHandle method)
        {
            var encInfo = GetEncMethodDebugInfo(methodBody);

            if (!encInfo.LocalSlots.IsDefaultOrEmpty)
            {
                var writer = new BlobBuilder();

                encInfo.SerializeLocalSlots(writer);

                _debugMetadataOpt.AddCustomDebugInformation(
                    parent: method,
                    kind: _debugMetadataOpt.GetOrAddGuid(PortableCustomDebugInfoKinds.EncLocalSlotMap),
                    value: _debugMetadataOpt.GetOrAddBlob(writer));
            }

            if (!encInfo.Lambdas.IsDefaultOrEmpty)
            {
                var writer = new BlobBuilder();

                encInfo.SerializeLambdaMap(writer);

                _debugMetadataOpt.AddCustomDebugInformation(
                    parent: method,
                    kind: _debugMetadataOpt.GetOrAddGuid(PortableCustomDebugInfoKinds.EncLambdaAndClosureMap),
                    value: _debugMetadataOpt.GetOrAddBlob(writer));
            }
        }

        #endregion
    }
}