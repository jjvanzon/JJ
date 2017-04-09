﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Validation.Resources;

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
            _alreadyDone = alreadyDone ?? throw new AlreadyDoneIsNullException();
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

            For(() => Obj.Amplifier, ResourceFormatter.Amplifier).IsNot(0.0);

            if (!Obj.IsActive)
            {
                ValidationMessages.Add(() => Obj.Amplifier, ResourceFormatter.NotActive);
            }

            if (_bytes == null)
            {
                ValidationMessages.Add(() => _bytes, ResourceFormatter.NotLoaded);
            }
            else if (_bytes.Length == 0)
            {
                ValidationMessages.Add(() => _bytes.Length, ValidationResourceFormatter.IsZero(CommonResourceFormatter.Count_WithNamePlural(ResourceFormatter.Samples)));
            }
        }
    }
}
