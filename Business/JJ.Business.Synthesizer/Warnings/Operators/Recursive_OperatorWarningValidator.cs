﻿using JJ.Business.Synthesizer.Extensions;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class Recursive_OperatorWarningValidator : ValidatorBase<Operator>
    {
        private ISampleRepository _sampleRepository;
        private HashSet<object> _alreadyDone;

        public Recursive_OperatorWarningValidator(Operator obj, ISampleRepository sampleRepository, HashSet<object> alreadyDone = null)
            : base(obj, postponeExecute: true)
        {
            if (sampleRepository == null) throw new NullException(() => sampleRepository);

            _sampleRepository = sampleRepository;
            _alreadyDone = alreadyDone ?? new HashSet<object>();

            Execute();
        }

        protected override void Execute()
        {
            if (_alreadyDone.Contains(Object)) return;
            _alreadyDone.Add(Object);

            ExecuteValidator(new Versatile_OperatorWarningValidator(Object));

            if (Object.GetOperatorTypeEnum() == OperatorTypeEnum.Sample)
            {
                int sampleID;
                if (Int32.TryParse(Object.Data, out sampleID))
                {
                    Sample sample = _sampleRepository.TryGet(sampleID);
                    if (sample != null)
                    {
                        byte[] bytes = _sampleRepository.TryGetBytes(sampleID);
                        ExecuteValidator(new SampleWarningValidator(sample, bytes, _alreadyDone));
                    }
                }
            }

            foreach (Inlet inlet in Object.Inlets)
            {
                if (inlet.InputOutlet != null)
                {
                    ExecuteValidator(new Recursive_OperatorWarningValidator(inlet.InputOutlet.Operator, _sampleRepository, _alreadyDone));
                }
            }
        }
    }
}
