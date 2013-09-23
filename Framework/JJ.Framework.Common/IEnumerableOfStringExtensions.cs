﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Framework.Common
{
    public static class IEnumerableOfStringExtensions
    {
        public static string[] Trim(this IEnumerable<string> values, params char[] trimChars)
        {
            return values.Select(x => x.Trim(trimChars)).ToArray();
        }
    }
}
