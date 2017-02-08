﻿using System;
using JetBrains.Annotations;

namespace JJ.Business.Synthesizer.Roslyn
{
    internal class ActivationInfo
    {
        public ActivationInfo([NotNull] Type type, [NotNull] object[] args)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (args == null) throw new ArgumentNullException(nameof(args));

            Type = type;
            Args = args;
        }

        [NotNull] public Type Type { get; }
        [NotNull] public object[] Args { get; }
    }
}