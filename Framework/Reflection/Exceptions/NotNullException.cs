﻿using System;
using System.Linq.Expressions;

namespace JJ.Framework.Reflection.Exceptions
{
    public class NotNullException : Exception
    {
        private const string MESSAGE = "{0} must be null.";

        public NotNullException(Expression<Func<object>> expression)
            : base(String.Format(MESSAGE, ExpressionHelper.GetText(expression)))
        { }
    }
}