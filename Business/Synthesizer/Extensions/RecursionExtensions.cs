﻿using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace JJ.Business.Synthesizer.Extensions
{
    public static class RecursionExtensions
    {
        /// <summary> Tells us whether an operator is circular within a patch. </summary>
        public static bool IsCircular(this Operator op)
        {
            if (op == null) throw new NullException(() => op);

            var alreadyDone = new HashSet<Operator>();

            return IsCircular(op, alreadyDone);
        }

        private static bool IsCircular(this Operator op, HashSet<Operator> alreadyDone)
        {
            // Be null-tollerant, because you might call it in places where the entities are not valid.
            if (op == null)
            {
                return false;
            }

            if (alreadyDone.Contains(op))
            {
                return true;
            }
            alreadyDone.Add(op);

            foreach (Inlet inlet in op.Inlets)
            {
                if (inlet.InputOutlet != null)
                {
                    if (IsCircular(inlet.InputOutlet.Operator, alreadyDone))
                    {
                        return true;
                    }
                }
            }

            alreadyDone.Remove(op);

            return false;
        }

        public static bool HasCircularUnderlyingPatch(this Document document, IPatchRepository patchRepository)
        {
            if (document == null) throw new NullException(() => document);

            return document.HasCircularUnderlyingPatch(patchRepository, new HashSet<object>());
        }

        public static bool HasCircularUnderlyingPatch(this Patch patch, IPatchRepository patchRepository)
        {
            if (patch == null) throw new NullException(() => patch);

            return patch.HasCircularUnderlyingPatch(patchRepository, new HashSet<object>());
        }

        public static bool HasCircularUnderlyingPatch(this Operator op, IPatchRepository patchRepository)
        {
            if (op == null) throw new NullException(() => op);
            if (op.GetOperatorTypeEnum() != OperatorTypeEnum.CustomOperator) throw new NotEqualException(() => op.GetOperatorTypeEnum(), OperatorTypeEnum.CustomOperator);

            return op.HasCircularUnderlyingPatch(patchRepository, new HashSet<object>());
        }

        private static bool HasCircularUnderlyingPatch(this Patch patch, IPatchRepository patchRepository, HashSet<object> alreadyDone)
        {
            if (alreadyDone.Contains(patch))
            {
                return true;
            }
            alreadyDone.Add(patch);

            IList<Operator> customOperators = patch.GetOperatorsOfType(OperatorTypeEnum.CustomOperator);
            foreach (Operator customOperator in customOperators)
            {
                if (customOperator.HasCircularUnderlyingPatch(patchRepository, alreadyDone))
                {
                    return true;
                }
            }

            alreadyDone.Remove(patch);

            return false;
        }

        private static bool HasCircularUnderlyingPatch(this Operator op, IPatchRepository patchRepository, HashSet<object> alreadyDone)
        {
            if (alreadyDone.Contains(op))
            {
                return true;
            }
            alreadyDone.Add(op);

            var wrapper = new CustomOperator_OperatorWrapper(op, patchRepository);
            Patch underlyingPatch = wrapper.UnderlyingPatch;

            if (underlyingPatch != null)
            {
                Document document = underlyingPatch.Document;
                if (document.HasCircularUnderlyingPatch(patchRepository, alreadyDone))
                {
                    return true;
                }
            }

            alreadyDone.Remove(op);

            return false;
        }

        private static bool HasCircularUnderlyingPatch(this Document document, IPatchRepository patchRepository, HashSet<object> alreadyDone)
        {
            if (alreadyDone.Contains(document))
            {
                return true;
            }
            alreadyDone.Add(document);

            foreach (Patch patch in document.Patches)
            {
                if (patch.HasCircularUnderlyingPatch(patchRepository, alreadyDone))
                {
                    return true;
                }
            }

            alreadyDone.Remove(document);

            return false;
        }

        /// <summary> Note that dependencies caused by library references (The DocumentReference entity) are not checked. </summary>
        public static IEnumerable<Operator> EnumerateDependentCustomOperators(this Patch patch, IPatchRepository patchRepository)
        {
            if (patch == null) throw new NullException(() => patch);
            if (patchRepository == null) throw new NullException(() => patchRepository);

            // In case of no document, there are no dependent custom operators.
            if (patch.Document == null)
            {
                return new Operator[0];
            }

            // We cannot use an SQL query, because that only operates on flushed / committed data.
            IEnumerable<Operator> enumerable = 
                patch.Document.Patches
                     .SelectMany(x => x.Operators)
                     .Where(x => x.GetOperatorTypeEnum() == OperatorTypeEnum.CustomOperator &&
                                 UnderlyingPatchIsMatch(patch, x, patchRepository));

            return enumerable;
        }

        private static bool UnderlyingPatchIsMatch(Patch underlyingPatch, Operator customOperator, IPatchRepository patchRepository)
        {
            var wrapper = new CustomOperator_OperatorWrapper(customOperator, patchRepository);

            Patch underlyingPatch2 = wrapper.UnderlyingPatch;

            if (underlyingPatch2 == null)
            {
                return false;
            }

            return underlyingPatch2.ID == underlyingPatch.ID;
        }

        /// <summary>  Should be same as patch.Operators, but in case of an invalid entity structure it might not be. </summary>
        public static IList<Operator> GetOperatorsRecursive(this Patch patch)
        {
            return EnumerateOperatorsRecursive(patch).ToArray();
        }

        /// <summary>  Should be same as patch.Operators, but in case of an invalid entity structure it might not be. </summary>
        public static IEnumerable<Operator> EnumerateOperatorsRecursive(this Patch patch)
        {
            var hashSet = new HashSet<Operator>();

            AddOperatorsInPatchRecursive(hashSet, patch);

            return hashSet;
        }

        private static void AddOperatorsInPatchRecursive(HashSet<Operator> hashSet, Patch patch)
        {
            if (patch == null) throw new NullException(() => patch);

            foreach (Operator op in patch.Operators)
            {
                AddOperatorsRecursive(hashSet, op);
            }
        }

        private static void AddOperatorsRecursive(HashSet<Operator> hashSet, Operator op)
        {
            if (hashSet.Contains(op))
            {
                return;
            }

            hashSet.Add(op);

            foreach (Inlet inlet in op.Inlets)
            {
                if (inlet.InputOutlet != null)
                {
                    AddOperatorsRecursive(hashSet, inlet.InputOutlet.Operator);
                }
            }

            foreach (Outlet outlet in op.Outlets)
            {
                foreach (Inlet inlet in outlet.ConnectedInlets)
                {
                    AddOperatorsRecursive(hashSet, inlet.InputOutlet.Operator);
                }
            }
        }
    }
}