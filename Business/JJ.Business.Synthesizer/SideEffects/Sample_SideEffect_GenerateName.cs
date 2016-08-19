﻿using JJ.Data.Synthesizer;
using JJ.Framework.Business;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Extensions;

namespace JJ.Business.Synthesizer.SideEffects
{
    internal class Sample_SideEffect_GenerateName : ISideEffect
    {
        private readonly Sample _entity;

        public Sample_SideEffect_GenerateName(Sample entity)
        {
            if (entity == null) throw new NullException(() => entity);
            if (entity.Document == null) throw new NullException(() => entity.Document);

            _entity = entity;
        }

        public void Execute()
        {
            IEnumerable<string> existingNames = _entity.Document.EnumerateSelfAndParentAndTheirChildren()
                                                                .SelectMany(x => x.Samples)
                                                                .Select(x => x.Name);

            _entity.Name = SideEffectHelper.GenerateName<Sample>(existingNames);
        }
    }
}
