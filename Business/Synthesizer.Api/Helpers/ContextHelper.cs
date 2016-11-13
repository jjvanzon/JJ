﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.Memory.Mappings;
using JJ.Framework.Data;
using JJ.Framework.Data.Memory;

namespace JJ.Business.Synthesizer.Api.Helpers
{
    internal static class ContextHelper
    {
        private static IContext _memoryContext = CreateMemoryContext();

        public static IContext MemoryContext { get { return _memoryContext; } }

        private static IContext CreateMemoryContext()
        {
            return new MemoryContext(null, typeof(Patch).Assembly, typeof(PatchMapping).Assembly);
        }
    }
}