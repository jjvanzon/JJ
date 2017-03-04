﻿using JJ.Framework.Common;
using JJ.Framework.Reflection;
using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace JJ.Framework.Validation
{
    internal static class PropertyKeyHelper
    {
        public static string GetPropertyKeyFromExpression([NotNull] Expression<Func<object>> propertyKeyExpression)
        {
            string propertyKey = ExpressionHelper.GetText(propertyKeyExpression, true);

            // Always cut off the root object, e.g. "MyObject.MyProperty" becomes "MyProperty".
            propertyKey = propertyKey.TrimStartUntil(".").TrimStart(".");

            return propertyKey;
        }
    }
}
