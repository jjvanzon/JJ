//
//  Circle.Framework.Integration.DotNet.Expressions.ExpressionHelper_UsingCustomExpressionVisitors_WithStack
//
//      Author: Jan-Joost van Zon
//      Date: 2013-02-12 - 2013-02-13
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;

namespace Circle.Framework.Integration.DotNet.Expressions
{
    public static class ExpressionHelper_UsingCustomExpressionVisitors_WithStack
    {
        // GetName

        public static string GetName(LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var translator = new ExpressionToNameTranslator2();
            translator.Visit(expression);
            return translator.Result;
        }

        // GetValue

        public static object GetValue(LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var translator = new ExpressionToValueTranslator2_UsingStack();
            translator.Visit(expression);
            return translator.Result;
        }
    }
}