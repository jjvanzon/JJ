﻿using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.Synthesizer.Names;
using JJ.Business.Synthesizer.Resources;

namespace JJ.Business.Synthesizer.Validation
{
    internal class RecursiveOperatorIsOfSamePatchOrPatchIsNullValidator : ValidatorBase<Operator>
    {
        private Patch _patch;

        public RecursiveOperatorIsOfSamePatchOrPatchIsNullValidator(Operator op, Patch patch)
            : base(op, postponeExecute: true)
        {
            if (patch == null) throw new NullException(() => patch);

            _patch = patch;
            Execute();
        }

        protected override void Execute()
        {
            if (Object.Patch != null &&
                Object.Patch != _patch)
            {
                ValidationMessages.Add(PropertyNames.Patch, MessageFormatter.OperatorPatchIsNotTheExpectedPatch(Object.Name, _patch.Name));
            }

            foreach (Inlet inlet in Object.Inlets)
            {
                if (inlet.InputOutlet != null)
                {
                    Execute(new RecursiveOperatorIsOfSamePatchOrPatchIsNullValidator(inlet.InputOutlet.Operator, _patch));
                }
            }
        }
    }
}
