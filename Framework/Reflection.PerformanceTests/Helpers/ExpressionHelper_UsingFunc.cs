﻿using System;
using JJ.Framework.Exceptions;

namespace JJ.OneOff.ExpressionTranslatorPerformanceTests.Helpers
{
    public static class ExpressionHelper_UsingFunc
    {
        // GetValue 

        public static T GetValue<T>(Func<T> expression)
        {
            if (expression == null)
            {
                throw new NullException(() => expression);
            }

            return expression();
        }
    }
}