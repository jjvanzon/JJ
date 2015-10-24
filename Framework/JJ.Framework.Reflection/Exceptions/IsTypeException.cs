﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JJ.Framework.Reflection.Exceptions
{
    public class IsTypeException : Exception
    {
        private const string MESSAGE_FORMAT = "{0} cannot be of type {1}.";

        private string _message;

        public override string Message
        {
            get { return _message; }
        }

        public IsTypeException(Expression<Func<object>> expression, Type type)
        {
            if (type == null) throw new NullException(() => type);

            _message = String.Format(MESSAGE_FORMAT, ExpressionHelper.GetText(expression), type.FullName);
        }

        public IsTypeException(Expression<Func<object>> expression, string typeName)
        {
            _message = String.Format(MESSAGE_FORMAT, ExpressionHelper.GetText(expression), typeName);
        }
    }
}