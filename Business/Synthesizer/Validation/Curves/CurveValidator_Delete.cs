﻿using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.Extensions;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace JJ.Business.Synthesizer.Validation.Curves
{
    internal class CurveValidator_Delete : VersatileValidator<Curve>
    {
        private readonly ICurveRepository _curveRepository;

        public CurveValidator_Delete([NotNull] Curve curve, [NotNull] ICurveRepository curveRepository)
            : base(curve, postponeExecute: true)
        {
            if (curveRepository == null) throw new NullException(() => curveRepository);

            _curveRepository = curveRepository;

            Execute();
        }

        protected sealed override void Execute()
        {
            Curve curve = Obj;

            bool hasOperators = EnumerateCurveOperators(curve).Any();
            if (hasOperators)
            {
                // TODO: It might be handy to know what patch and possibly what operator still uses it.
                ValidationMessages.Add(PropertyNames.Curve, MessageFormatter.CannotDeleteCurveBecauseHasOperators(curve.Name));
            }
        }

        private IEnumerable<Operator> EnumerateCurveOperators([NotNull] Curve curve)
        {
            if (curve == null) throw new NullException(() => curve);
            if (curve.Document == null)
            {
                yield break;    
            }

            foreach (Operator op in curve.Document.Patches.SelectMany(x => x.Operators))
            {
                if (op.GetOperatorTypeEnum() != OperatorTypeEnum.Curve)
                {
                    continue;
                }

                var wrapper = new Curve_OperatorWrapper(op, _curveRepository);

                if (wrapper.Curve == curve ||
                    wrapper.CurveID == curve.ID)
                {
                    yield return op;
                }
            }
        }
    }
}
