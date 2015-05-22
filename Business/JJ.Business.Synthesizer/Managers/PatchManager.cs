﻿using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.Validation;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.LinkTo;

namespace JJ.Business.Synthesizer.Managers
{
    public static class PatchManager
    {
        /// <summary>
        /// Adds an operator to the patch.
        /// Related operators will also be added to the patch.
        /// If one of the related operators has a different patch assigned to it,
        /// a validation message is returned.
        /// </summary>
        public static VoidResult AddToPatchRecursive(Operator op, Patch patch)
        {
            if (op == null) throw new NullException(() => op);
            if (patch == null) throw new NullException(() => patch);

            IValidator validator = new OperatorValidator_Recursive_IsOfSamePatchOrPatchIsNull(op, patch);
            if (!validator.IsValid)
            {
                return new VoidResult
                {
                    Successful = false,
                    Messages = validator.ValidationMessages.ToCanonical()
                };
            }

            FillInPatchRecursive(op, patch);

            return new VoidResult
            {
                Successful = true,
                Messages = new JJ.Business.CanonicalModel.Message[0]
            };
        }

        private static void FillInPatchRecursive(Operator op, Patch patch)
        {
            op.LinkTo(patch);

            foreach (Inlet inlet in op.Inlets)
            {
                if (inlet.InputOutlet != null)
                {
                    FillInPatchRecursive(inlet.InputOutlet.Operator, patch);
                }
            }
        }
    }
}
