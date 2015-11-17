﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Framework.Business;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Linq;
using JJ.Business.Synthesizer.Extensions;
using System.Collections.Generic;
using JJ.Framework.Common;

namespace JJ.Business.Synthesizer.SideEffects
{
    internal class Curve_SideEffect_GenerateName : ISideEffect
    {
        private Curve _entity;

        public Curve_SideEffect_GenerateName(Curve entity)
        {
            if (entity == null) throw new NullException(() => entity);
            _entity = entity;
        }

        public void Execute()
        {
            if (_entity.Document == null) throw new NullException(() => _entity.Document);

            HashSet<string> existingNames = _entity.Document.EnumerateSelfAndParentAndTheirChildren()
                                                            .SelectMany(x => x.Curves)
                                                            .Select(x => x.Name)
                                                            .ToHashSet();
            int number = 1;
            string suggestedName;
            bool nameExists;

            do
            {
                suggestedName = String.Format("{0} {1}", PropertyDisplayNames.Curve, number++);
                nameExists = existingNames.Contains(suggestedName);
            }
            while (nameExists);

            _entity.Name = suggestedName;
        }
    }
}
