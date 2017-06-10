﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Extensions;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;
using System.Collections.Generic;
using JetBrains.Annotations;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Validation.Curves;
using JJ.Business.Synthesizer.Validation.Samples;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class OperatorValidator_WithUnderlyingEntities : ValidatorBase<Operator>
    {
        private readonly ICurveRepository _curveRepository;
        private readonly ISampleRepository _sampleRepository;
        private readonly HashSet<object> _alreadyDone;

        /// <summary>
        /// Validates an operator, but not its descendant operators.
        /// Does validate underlying curves and samples.
        /// Makes sure that objects are only validated once to 
        /// prevent excessive validation messages.
        /// The reason that underlying entities such as samples and curves are validated here,
        /// is because even though it already happens when you validate a whole document,
        /// in some cases you do not validate the whole document, but a narrower scope,
        /// such as a patch.
        /// </summary>
        public OperatorValidator_WithUnderlyingEntities(
            [NotNull] Operator obj,
            [NotNull] ICurveRepository curveRepository,
            [NotNull] ISampleRepository sampleRepository,
            [NotNull] HashSet<object> alreadyDone)
            : base(obj, postponeExecute: true)
        {
            _curveRepository = curveRepository ?? throw new NullException(() => curveRepository);
            _sampleRepository = sampleRepository ?? throw new NullException(() => sampleRepository);
            _alreadyDone = alreadyDone ?? throw new AlreadyDoneIsNullException();

            Execute();
        }

        protected sealed override void Execute()
        {
            Operator op = Obj;

            if (_alreadyDone.Contains(op))
            {
                return;
            }
            _alreadyDone.Add(op);

            ExecuteValidator(new Versatile_OperatorValidator(op));

            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

            if (operatorTypeEnum == OperatorTypeEnum.Curve)
            {
                int curveID;
                if (int.TryParse(op.Data, out curveID))
                {
                    Curve curve = _curveRepository.TryGet(curveID);
                    if (curve != null)
                    {
                        if (!_alreadyDone.Contains(curve))
                        {
                            _alreadyDone.Add(curve);

                            string curveMessagePrefix = ValidationHelper.GetMessagePrefix(curve);

                            ExecuteValidator(new CurveValidator_WithoutNodes(curve), curveMessagePrefix);
                            ExecuteValidator(new CurveValidator_Nodes(curve), curveMessagePrefix);
                        }
                    }
                }
            }

            if (operatorTypeEnum == OperatorTypeEnum.Sample)
            {
                int sampleID;
                if (int.TryParse(op.Data, out sampleID))
                {
                    Sample sample = _sampleRepository.TryGet(sampleID);
                    if (sample != null)
                    {
                        if (!_alreadyDone.Contains(sample))
                        {
                            _alreadyDone.Add(sample);
                            ExecuteValidator(new SampleValidator(sample), ValidationHelper.GetMessagePrefix(sample));
                        }
                    }
                }
            }
        }
    }
}
