﻿using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.Extensions;
using JJ.Data.Synthesizer;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class OperatorValidator_IsCircular : VersatileValidator<Operator>
    {
        private readonly IPatchRepository _patchRepository;

        public OperatorValidator_IsCircular(Operator op, IPatchRepository patchRepository)
            : base(op, postponeExecute: true)
        {
            if (patchRepository == null) throw new NullException(() => patchRepository);

            _patchRepository = patchRepository;

            Execute();
        }

        protected sealed override void Execute()
        {
            Operator op = Obj;

            if (op.IsCircular())
            {
                ValidationMessages.Add(() => op, ResourceFormatter.CircularReference);
            }

            // TODO: Enable the UnderlyingPatchIsCircular check again, when it is corrected, so it works.
            return;

            // ReSharper disable once HeuristicUnreachableCode
            // ReSharper disable once InvertIf
            if (op.GetOperatorTypeEnum() == OperatorTypeEnum.CustomOperator)
            {
                if (op.HasCircularUnderlyingPatch(_patchRepository))
                {
                    ValidationMessages.Add(() => op, ResourceFormatter.UnderlyingPatchIsCircular);
                }
            }
        }
    }
}
