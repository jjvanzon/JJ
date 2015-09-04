﻿using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Warnings
{
    public class PatchWarningValidator_Recursive : FluentValidator<Patch>
    {
        private ISampleRepository _sampleRepository;

        private HashSet<object> _alreadyDone;

        public PatchWarningValidator_Recursive(Patch obj, ISampleRepository sampleRepository, HashSet<object> alreadyDone)
            : base(obj, postponeExecute: true)
        {
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (alreadyDone == null) throw new AlreadyDoneIsNullException();

            _sampleRepository = sampleRepository;
            _alreadyDone = alreadyDone;

            Execute();
        }

        protected override void Execute()
        {
            foreach (Operator op in Object.Operators)
            {
                Execute(new OperatorWarningValidator_Recursive(op, _sampleRepository, _alreadyDone));
            }
        }
    }
}
