﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.SideEffects
{
    internal static class SideEffectHelper
    {
        public static string GenerateName<TEntity>(IEnumerable<string> existingNames)
        {
            if (existingNames == null) throw new NullException(() => existingNames);

            HashSet<string> canonicalExistingNamesHashSet = existingNames.Select(x => NameHelper.ToCanonical(x)).ToHashSet();

            int number = 1;
            string suggestedName;
            bool nameExists;

            string entityDisplayName = ResourceHelper.GetPropertyDisplayName(typeof(TEntity).Name);

            do
            {
                suggestedName = String.Format("{0} {1}", entityDisplayName, number++);
                nameExists = canonicalExistingNamesHashSet.Contains(NameHelper.ToCanonical(suggestedName));
            }
            while (nameExists);

            return suggestedName;
        }
    }
}