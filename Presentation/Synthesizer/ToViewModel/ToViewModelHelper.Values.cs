﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.Synthesizer;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;
using JJ.Framework.Collections;
using JJ.Framework.Configuration;
using JJ.Framework.Exceptions;
using JJ.Framework.Mathematics;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Items;
// ReSharper disable RedundantCaseLabel

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    internal static partial class ToViewModelHelper
    {
        public const string DIMENSION_KEY_EMPTY = "";
        public const string STANDARD_DIMENSION_KEY_PREFIX = "0C26ADA8-0BFC-484C-BF80-774D055DAA3F-StandardDimension-";
        public const string CUSTOM_DIMENSION_KEY_PREFIX = "5133584A-BA76-42DB-BD0E-42801FCB96DF-CustomDimension-";

        private static readonly bool _idsVisible = CustomConfigurationManager.GetSection<ConfigurationSection>().IDsVisible;

        [Obsolete("Will be replaced with Inlet.NameOrDimensionHidden, " +
                  "but only after all standard operators have been bootstrapped " +
                  "into the System Document.")]
        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithHiddenInletNames { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.PatchInlet,
                OperatorTypeEnum.PatchOutlet,
                OperatorTypeEnum.Number,
                OperatorTypeEnum.Curve,
                OperatorTypeEnum.Noise,
                OperatorTypeEnum.GetDimension,
                OperatorTypeEnum.Hold,
                OperatorTypeEnum.DimensionToOutlets,
                OperatorTypeEnum.InletsToDimension
            };

        [Obsolete("Will be replaced with Outlet.NameOrDimensionHidden, " +
                  "but only after all standard operators have been bootstrapped " +
                  "into the System Document.")]
        // A list until it will have more items. Then it might be made a HashSet for performance.
        public static IList<OperatorTypeEnum> OperatorTypeEnums_WithVisibleOutletNames { get; } =
            new List<OperatorTypeEnum>
            {
                OperatorTypeEnum.ChangeTrigger,
                OperatorTypeEnum.CustomOperator,
                OperatorTypeEnum.PulseTrigger,
                OperatorTypeEnum.Reset,
                OperatorTypeEnum.SetDimension,
                OperatorTypeEnum.ToggleTrigger,
            };

        // Dimensions

        public static string GetDimensionKey(Operator op)
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!string.IsNullOrEmpty(op.CustomDimensionName))
            {
                return $"{CUSTOM_DIMENSION_KEY_PREFIX}{op.CustomDimensionName}";
            }
            else
            {
                return GetDimensionKey(op.GetStandardDimensionEnum());
            }
        }

        public static string GetDimensionKey(DimensionEnum standardDimensionEnum)
        {
            if (standardDimensionEnum != DimensionEnum.Undefined)
            {
                return $"{STANDARD_DIMENSION_KEY_PREFIX}{standardDimensionEnum}";
            }

            return DIMENSION_KEY_EMPTY;
        }

        public static string TryGetDimensionName(Operator op)
        {
            if (!string.IsNullOrEmpty(op.CustomDimensionName))
            {
                return op.CustomDimensionName;
            }

            DimensionEnum standardDimensionEnum = op.GetStandardDimensionEnum();
            if (standardDimensionEnum != DimensionEnum.Undefined)
            {
                return ResourceFormatter.GetDisplayName(standardDimensionEnum);
            }

            return null;
        }

        public static bool MustStyleDimension(Operator entity)
        {
            switch (entity.GetOperatorTypeEnum())
            {
                case OperatorTypeEnum.GetDimension:
                case OperatorTypeEnum.SetDimension:
                    return false;
            }

            bool hasDimension = entity.HasDimension || (entity.OperatorType?.HasDimension ?? false); // Excuse the smell of polymorphism: OperatorType will be deprecated at some point.
            
            return hasDimension;
        }

        // Document

        public static bool GetCanCreateNew(DocumentTreeNodeTypeEnum selectedNodeType, bool patchDetailsVisible)
        {
            if (!patchDetailsVisible)
            {
                return false;
            }

            switch (selectedNodeType)
            {
                case DocumentTreeNodeTypeEnum.LibraryPatch:
                case DocumentTreeNodeTypeEnum.Patch:
                    return true;

                case DocumentTreeNodeTypeEnum.AudioOutput:
                case DocumentTreeNodeTypeEnum.AudioFileOutputList:
                case DocumentTreeNodeTypeEnum.Curves:
                case DocumentTreeNodeTypeEnum.Libraries:
                case DocumentTreeNodeTypeEnum.Library:
                case DocumentTreeNodeTypeEnum.LibraryPatchGroup:
                case DocumentTreeNodeTypeEnum.PatchGroup:
                case DocumentTreeNodeTypeEnum.Samples:
                case DocumentTreeNodeTypeEnum.Scales:
                default:
                    return false;
            }
        }

        public static bool GetCanPlay(DocumentTreeNodeTypeEnum selectedNodeType)
        {
            switch (selectedNodeType)
            {
                case DocumentTreeNodeTypeEnum.AudioOutput:
                case DocumentTreeNodeTypeEnum.Library:
                case DocumentTreeNodeTypeEnum.LibraryPatch:
                case DocumentTreeNodeTypeEnum.LibraryPatchGroup:
                case DocumentTreeNodeTypeEnum.Patch:
                case DocumentTreeNodeTypeEnum.PatchGroup:
                case DocumentTreeNodeTypeEnum.Samples:
                case DocumentTreeNodeTypeEnum.Libraries:
                    return true;

                case DocumentTreeNodeTypeEnum.AudioFileOutputList:
                case DocumentTreeNodeTypeEnum.Curves:
                case DocumentTreeNodeTypeEnum.Scales:
                default:
                    return false;
            }
        }

        public static bool GetCanOpenExternally(DocumentTreeNodeTypeEnum selectedNodeType)
        {
            switch (selectedNodeType)
            {
                case DocumentTreeNodeTypeEnum.Library:
                case DocumentTreeNodeTypeEnum.LibraryPatch:
                    return true;

                case DocumentTreeNodeTypeEnum.AudioFileOutputList:
                case DocumentTreeNodeTypeEnum.AudioOutput:
                case DocumentTreeNodeTypeEnum.Curves:
                case DocumentTreeNodeTypeEnum.LibraryPatchGroup:
                case DocumentTreeNodeTypeEnum.Libraries:
                case DocumentTreeNodeTypeEnum.Patch:
                case DocumentTreeNodeTypeEnum.PatchGroup:
                case DocumentTreeNodeTypeEnum.Samples:
                case DocumentTreeNodeTypeEnum.Scales:
                default:
                    return false;
            }
        }

        // Inlet

        public static bool GetInletVisible(Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            if (inlet.InputOutlet != null)
            {
                return true;
            }

            Operator op = inlet.Operator;

            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

            switch (operatorTypeEnum)
            {
                case OperatorTypeEnum.PatchInlet:
                    return false;

                case OperatorTypeEnum.Stretch:
                case OperatorTypeEnum.Squash:
                    if (inlet.GetDimensionEnum() == DimensionEnum.Origin)
                    {
                        if (op.GetStandardDimensionEnum() == DimensionEnum.Time)
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        public static string GetInletCaption(
            Inlet inlet,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository)
        {
            var sb = new StringBuilder();

            OperatorTypeEnum operatorTypeEnum = inlet.Operator.GetOperatorTypeEnum();
            bool nameOrDimensionHidden;
            if (inlet.Operator.UnderlyingPatch != null)
            {
                nameOrDimensionHidden = inlet.NameOrDimensionHidden;
            }
            else
            {
                nameOrDimensionHidden = OperatorTypeEnums_WithHiddenInletNames.Contains(operatorTypeEnum);
            }

            if (!nameOrDimensionHidden)
            {
                // Name or Dimension
                OperatorWrapperBase wrapper = EntityWrapperFactory.CreateOperatorWrapper(
                    inlet.Operator,
                    curveRepository,
                    sampleRepository);
                string inletDisplayName = wrapper.GetInletDisplayName(inlet);
                sb.Append(inletDisplayName);

                // RepetitionPosition
                if (inlet.RepetitionPosition.HasValue)
                {
                    sb.Append($" {inlet.RepetitionPosition + 1}");
                }
            }

            // DefaultValue
            if (inlet.InputOutlet == null)
            {
                if (inlet.DefaultValue.HasValue)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(' ');
                    }
                    sb.AppendFormat("= {0:0.####}", inlet.DefaultValue.Value);
                }
            }

            // IsObsolete
            if (inlet.IsObsolete)
            {
                AppendObsoleteFlag(sb);
            }

            // ID
            if (_idsVisible)
            {
                sb.Append($" ({inlet.ID})");
            }

            return sb.ToString();
        }

        public static float? TryGetConnectionDistance(Inlet entity, EntityPositionManager entityPositionManager)
        {
            if (entity == null) throw new NullException(() => entity);

            if (entity.InputOutlet == null)
            {
                return null;
            }

            Operator operator1 = entity.Operator;
            Operator operator2 = entity.InputOutlet.Operator;

            EntityPosition entityPosition1 = entityPositionManager.GetOrCreateOperatorPosition(operator1.ID);
            EntityPosition entityPosition2 = entityPositionManager.GetOrCreateOperatorPosition(operator2.ID);

            float distance = Geometry.AbsoluteDistance(
                entityPosition1.X,
                entityPosition1.Y,
                entityPosition2.X,
                entityPosition2.Y);

            return distance;
        }

        // Node

        public static string GetNodeCaption(Node entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return $"{entity.X:0.####}, {entity.Y:0.####}";
        }

        // Operator

        /// <summary>
        /// A Number Operator can be considered 'owned' by another operator if
        /// it is the only operator it is connected to.
        /// In that case it is convenient that the Number Operator moves along
        /// with the operator it is connected to.
        /// In the Vector Graphics we accomplish this by making the Number Operator Rectangle a child of the owning Operator's Rectangle. 
        /// But also in the MoveOperator action we must move the owned operators along with their owner.
        /// </summary>
        public static bool GetOperatorIsOwned(Operator entity)
        {
            // ReSharper disable once InvertIf
            if (entity.Outlets.Count > 0)
            {
                bool isOwned = entity.GetOperatorTypeEnum() == OperatorTypeEnum.Number &&
                               // Make sure the connected inlets are all of the same operator.
                               entity.Outlets[0].ConnectedInlets.Select(x => x.Operator).Distinct().Count() == 1;

                return isOwned;
            }

            return false;
        }

        /// <summary>
        /// In the future, when all operator types are bootstrapped into System Patches
        /// and no OperatorType entity exists anymore, this method will be obsolete.
        /// </summary>
        public static bool GetCanSelectUnderlyingPatch(Operator entity)
        {
            if (entity.OperatorType == null)
            {
                return true;
            }

            if (OperatorTypeEnums_WithStandardPropertiesView.Contains(entity.GetOperatorTypeEnum()))
            {
                return true;
            }

            return false;
        }

        public static string GetOperatorCaption(
            Operator op,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository)
        {
            if (op == null) throw new NullException(() => op);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (curveRepository == null) throw new NullException(() => curveRepository);

            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

            string operatorCaption;

            switch (operatorTypeEnum)
            {
                case OperatorTypeEnum.AverageOverDimension:
                case OperatorTypeEnum.AverageOverInlets:
                    operatorCaption = ResourceFormatter.Average;
                    break;

                case OperatorTypeEnum.ClosestOverDimension:
                case OperatorTypeEnum.ClosestOverInlets:
                    operatorCaption = ResourceFormatter.Closest;
                    break;

                case OperatorTypeEnum.ClosestOverDimensionExp:
                case OperatorTypeEnum.ClosestOverInletsExp:
                    operatorCaption = ResourceFormatter.ClosestExp;
                    break;

                case OperatorTypeEnum.Curve:
                    operatorCaption = GetOperatorCaption_ForCurve(op, curveRepository);
                    break;

                case OperatorTypeEnum.CustomOperator:
                    operatorCaption = GetOperatorCaption_ForCustomOperator(op);
                    break;

                case OperatorTypeEnum.GetDimension:
                    operatorCaption = GetOperatorCaption_ForGetDimension(op);
                    break;

                case OperatorTypeEnum.MaxOverDimension:
                case OperatorTypeEnum.MaxOverInlets:
                    operatorCaption = ResourceFormatter.Max;
                    break;

                case OperatorTypeEnum.MinOverDimension:
                case OperatorTypeEnum.MinOverInlets:
                    operatorCaption = ResourceFormatter.Min;
                    break;

                case OperatorTypeEnum.Number:
                    operatorCaption = GetOperatorCaption_ForNumber(op);
                    break;

                case OperatorTypeEnum.PatchInlet:
                    operatorCaption = GetOperatorCaption_ForPatchInlet(op);
                    break;

                case OperatorTypeEnum.PatchOutlet:
                    operatorCaption = GetOperatorCaption_ForPatchOutlet(op);
                    break;

                case OperatorTypeEnum.RangeOverDimension:
                case OperatorTypeEnum.RangeOverOutlets:
                    operatorCaption = ResourceFormatter.Range;
                    break;

                case OperatorTypeEnum.Sample:
                    operatorCaption = GetOperatorCaption_ForSample(op, sampleRepository);
                    break;

                case OperatorTypeEnum.SetDimension:
                    operatorCaption = GetOperatorCaption_ForSetDimension(op);
                    break;

                case OperatorTypeEnum.SortOverDimension:
                case OperatorTypeEnum.SortOverInlets:
                    operatorCaption = ResourceFormatter.Sort;
                    break;

                case OperatorTypeEnum.SumOverDimension:
                    operatorCaption = ResourceFormatter.Sum;
                    break;

                default:
                    operatorCaption = GetOperatorCaption_ForOtherOperators(op);
                    break;
            }

            if (_idsVisible)
            {
                operatorCaption += $" ({op.ID})";
            }

            return operatorCaption;
        }

        private static string GetOperatorCaption_ForCurve(Operator op, ICurveRepository curveRepository)
        {
            string operatorTypeDisplayName = ResourceFormatter.Curve;

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{operatorTypeDisplayName}: {op.Name}";
            }

            // Use Curve.Name
            var wrapper = new Curve_OperatorWrapper(op, curveRepository);
            Curve underlyingEntity = wrapper.Curve;
            if (!string.IsNullOrWhiteSpace(underlyingEntity?.Name))
            {
                return $"{operatorTypeDisplayName}: {underlyingEntity.Name}";
            }

            // Use OperatorTypeDisplayName
            string caption = operatorTypeDisplayName;
            return caption;
        }

        private static string GetOperatorCaption_ForCustomOperator(Operator op)
        {
            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return op.Name;
            }

            // Use OperatorTypeDisplayName
            string caption = ResourceFormatter.GetUnderlyingPatchDisplayName_OrOperatorTypeDisplayName(op);
            return caption;
        }

        /// <summary> Value Operator: display name and value or only value. </summary>
        private static string GetOperatorCaption_ForNumber(Operator op)
        {
            var wrapper = new Number_OperatorWrapper(op);
            string formattedValue = wrapper.Number.ToString("0.####");

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (string.IsNullOrWhiteSpace(op.Name))
            {
                return formattedValue;
            }
            else
            {
                return $"{op.Name}: {formattedValue}";
            }
        }

        private static string GetOperatorCaption_ForPatchInlet(Operator op)
        {
            var sb = new StringBuilder();

            var wrapper = new PatchInlet_OperatorWrapper(op);
            string name = wrapper.Inlet.Name;
            DimensionEnum dimensionEnum = wrapper.Inlet.GetDimensionEnum();

            // Use OperatorType DisplayName
            sb.Append(ResourceFormatter.Inlet);

            // Try Use Operator Name
            if (!string.IsNullOrWhiteSpace(name))
            {
                sb.Append($": {name}");
            }
            // Try Use Dimension
            else if (dimensionEnum != DimensionEnum.Undefined)
            {
                sb.Append($": {ResourceFormatter.GetDisplayName(dimensionEnum)}");
            }
            // Try Use List Index
            else
            {
                sb.Append($" {wrapper.Inlet.Position}");
            }

            // Try Use DefaultValue
            double? defaultValue = wrapper.Inlet.DefaultValue;
            if (defaultValue.HasValue)
            {
                sb.Append($" = {defaultValue.Value}");
            }

            return sb.ToString();
        }

        private static string GetOperatorCaption_ForPatchOutlet(Operator op)
        {
            var sb = new StringBuilder();

            var wrapper = new PatchOutlet_OperatorWrapper(op);
            Outlet outlet = wrapper.Outlet;
            string name = outlet.Name;
            DimensionEnum dimensionEnum = outlet.GetDimensionEnum();

            // Use OperatorType DisplayName
            sb.Append(ResourceFormatter.Outlet);

            // Try Use Operator Name
            if (!string.IsNullOrWhiteSpace(name))
            {
                sb.Append($": {name}");
            }
            // Try Use Dimension
            else if (dimensionEnum != DimensionEnum.Undefined)
            {
                sb.AppendFormat(": {0}", ResourceFormatter.GetDisplayName(dimensionEnum));
            }
            // Try Use List Index
            else
            {
                sb.AppendFormat(" {0}", outlet.Position);
            }

            return sb.ToString();
        }

        private static string GetOperatorCaption_ForSample(Operator op, ISampleRepository sampleRepository)
        {
            string operatorTypeDisplayName = ResourceFormatter.Sample;

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{operatorTypeDisplayName}: {op.Name}";
            }

            // Use Sample.Name
            var wrapper = new Sample_OperatorWrapper(op, sampleRepository);
            Sample underlyingEntity = wrapper.Sample;
            if (!string.IsNullOrWhiteSpace(underlyingEntity?.Name))
            {
                return $"{operatorTypeDisplayName}: {underlyingEntity.Name}";
            }

            // Use OperatorType DisplayName
            string caption = operatorTypeDisplayName;
            return caption;
        }

        private static string GetOperatorCaption_ForGetDimension(Operator op)
        {
            return GetOperatorCaption_WithDimensionPlaceholder(
                op,
                ResourceFormatter.GetDimensionWithPlaceholder("{0}")); // HACK: Method delegated to will replace placeholder.
        }

        private static string GetOperatorCaption_ForSetDimension(Operator op)
        {
            return GetOperatorCaption_WithDimensionPlaceholder(
                op,
                ResourceFormatter.SetDimensionWithPlaceholder("{0}")); // HACK: Method delegated to will replace placeholder.
        }

        private static string GetOperatorCaption_WithDimensionPlaceholder(Operator op, string operatorTypeDisplayNameWithPlaceholder)
        {
            DimensionEnum dimensionEnum = op.GetStandardDimensionEnum();
            string formattedOperatorTypeDisplayName;
            if (dimensionEnum != DimensionEnum.Undefined)
            {
                string dimensionDisplayName = ResourceFormatter.GetDisplayName(dimensionEnum);
                formattedOperatorTypeDisplayName = string.Format(operatorTypeDisplayNameWithPlaceholder, dimensionDisplayName);
            }
            else
            {
                formattedOperatorTypeDisplayName = ResourceFormatter.GetUnderlyingPatchDisplayName_OrOperatorTypeDisplayName(op);
            }

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{formattedOperatorTypeDisplayName}: {op.Name}";
            }
            // Use OperatorType DisplayName only.
            else
            {
                return formattedOperatorTypeDisplayName;
            }
        }

        private static string GetOperatorCaption_ForOtherOperators(Operator op)
        {
            string operatorTypeDisplayName = ResourceFormatter.GetUnderlyingPatchDisplayName_OrOperatorTypeDisplayName(op);

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{operatorTypeDisplayName}: {op.Name}";
            }

            // Use OperatorType DisplayName
            string caption = operatorTypeDisplayName;
            return caption;
        }

        // Outlet

        public static bool GetOutletVisible(Outlet outlet)
        {
            if (outlet == null) throw new NullException(() => outlet);

            OperatorTypeEnum operatorTypeEnum = outlet.Operator.GetOperatorTypeEnum();

            if (operatorTypeEnum == OperatorTypeEnum.PatchOutlet)
            {
                return false;
            }

            return true;
        }

        public static string GetOutletCaption(Outlet outlet, ISampleRepository sampleRepository, ICurveRepository curveRepository)
        {
            if (outlet == null) throw new NullException(() => outlet);

            switch (outlet.Operator.GetOperatorTypeEnum())
            {
                case OperatorTypeEnum.RangeOverOutlets:
                    return GetOutletCaption_ForRangeOverOutlets(outlet);

                default:
                    return GetOutletCaption_ForOtherOperatorType(outlet, sampleRepository, curveRepository);
            }
        }

        private static string GetOutletCaption_ForRangeOverOutlets(Outlet outlet)
        {
            var sb = new StringBuilder();

            double? from = outlet.Operator.Inlets
                                 .Where(x => x.GetDimensionEnum() == DimensionEnum.From)
                                 .Select(x => x.TryGetConstantNumber())
                                 .FirstOrDefault();
            if (@from.HasValue)
            {
                double? step = outlet.Operator.Inlets
                                     .Where(x => x.GetDimensionEnum() == DimensionEnum.Step)
                                     .Select(x => x.TryGetConstantNumber())
                                     .FirstOrDefault();
                if (step.HasValue)
                {
                    int listIndex = outlet.Operator.Outlets.IndexOf(outlet);

                    double value = @from.Value + step.Value * listIndex;

                    sb.Append(value.ToString("0.####"));
                }
            }

            if (outlet.IsObsolete)
            {
                AppendObsoleteFlag(sb);
            }

            if (_idsVisible)
            {
                sb.Append($" ({outlet.ID})");
            }

            return sb.ToString();
        }

        private static string GetOutletCaption_ForOtherOperatorType(
            Outlet outlet,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository)
        {
            var sb = new StringBuilder();

            OperatorTypeEnum operatorTypeEnum = outlet.Operator.GetOperatorTypeEnum();

            bool nameOrDimensionHidden;
            if (outlet.Operator.UnderlyingPatch != null)
            {
                nameOrDimensionHidden = outlet.NameOrDimensionHidden;
            }
            else
            {
                nameOrDimensionHidden = !OperatorTypeEnums_WithVisibleOutletNames.Contains(operatorTypeEnum);
            }

            if (!nameOrDimensionHidden)
            {
                // Dimension or Name
                OperatorWrapperBase wrapper = EntityWrapperFactory.CreateOperatorWrapper(outlet.Operator, curveRepository, sampleRepository);
                string inletDisplayName = wrapper.GetOutletDisplayName(outlet);
                sb.Append(inletDisplayName);

                // RepetitionPosition
                if (outlet.RepetitionPosition.HasValue)
                {
                    sb.Append($" {outlet.RepetitionPosition + 1}");
                }
            }

            // IsObsolete
            if (outlet.IsObsolete)
            {
                AppendObsoleteFlag(sb);
            }

            // ID
            if (_idsVisible)
            {
                sb.Append($" ({outlet.ID})");
            }

            return sb.ToString();
        }

        public static float? TryGetAverageConnectionDistance(Outlet entity, EntityPositionManager entityPositionManager)
        {
            if (entity == null) throw new NullException(() => entity);

            int connectedInletCount = entity.ConnectedInlets.Count;

            if (connectedInletCount == 0)
            {
                return null;
            }

            Operator operator1 = entity.Operator;

            float aggregate = 0f;

            foreach (Inlet connectedInlet in entity.ConnectedInlets)
            {
                Operator operator2 = connectedInlet.Operator;

                EntityPosition entityPosition1 = entityPositionManager.GetOrCreateOperatorPosition(operator1.ID);
                EntityPosition entityPosition2 = entityPositionManager.GetOrCreateOperatorPosition(operator2.ID);

                float distance = Geometry.AbsoluteDistance(
                    entityPosition1.X,
                    entityPosition1.Y,
                    entityPosition2.X,
                    entityPosition2.Y);

                aggregate += distance;
            }

            aggregate /= connectedInletCount;

            return aggregate;
        }

        // Tone

        public static string GetToneGridEditNumberTitle(Scale entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return ResourceFormatter.GetScaleTypeDisplayNameSingular(entity);
        }

        // Helpers

        private static void AppendObsoleteFlag(StringBuilder sb)
        {
            if (sb.Length != 0)
            {
                sb.Append(' ');
            }

            sb.AppendFormat("({0})", ResourceFormatter.IsObsolete);
        }
    }
}