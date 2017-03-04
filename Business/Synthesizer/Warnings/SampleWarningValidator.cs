﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class SampleWarningValidator : VersatileValidator<Sample>
    {
        private readonly byte[] _bytes;
        private readonly HashSet<object> _alreadyDone;

        /// <param name="bytes">nullable</param>
        public SampleWarningValidator(Sample obj, byte[] bytes, HashSet<object> alreadyDone)
            : base(obj, postponeExecute: true)
        {
            if (alreadyDone == null) throw new AlreadyDoneIsNullException();

            _alreadyDone = alreadyDone;
            _bytes = bytes;

            // ReSharper disable once VirtualMemberCallInConstructor
            Execute();
        }

        protected override void Execute()
        {
            if (_alreadyDone.Contains(Obj))
            {
                return;
            }
            _alreadyDone.Add(Obj);

            For(() => Obj.Amplifier, PropertyDisplayNames.Amplifier).IsNot(0.0);

            if (!Obj.IsActive)
            {
                ValidationMessages.Add(() => Obj.Amplifier, MessageFormatter.SampleNotActive(Obj.Name));
            }

            if (_bytes == null)
            {
                ValidationMessages.Add(() => _bytes, MessageFormatter.SampleNotLoaded(Obj.Name));
            }
            else if (_bytes.Length == 0)
            {
                ValidationMessages.Add(() => _bytes.Length, MessageFormatter.SampleCount0(Obj.Name));
            }
        }
    }
}
