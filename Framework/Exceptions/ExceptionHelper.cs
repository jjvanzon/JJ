﻿using System;
using System.Linq.Expressions;
using JJ.Framework.Reflection;

namespace JJ.Framework.Exceptions
{
	internal static class ExceptionHelper
	{
		/// <param name="type">nullable</param>
		public static string TryFormatFullTypeName(Type type) => type == null ? "<null>" : type.FullName;

		/// <param name="type">nullable</param>
		public static string TryFormatShortTypeName(Type type) => type == null ? "<null>" : type.Name;

		public static string FormatValue(object value) => value == null ? "<null>" : $"{value}";

		public static string GetTextWithValue(Expression<Func<object>> expression)
		{
			string text = ExpressionHelper.GetText(expression);
			object value = ExpressionHelper.GetValue(expression);
			bool mustShowValue = ReflectionHelper.IsSimpleType(value) && !string.IsNullOrEmpty(Convert.ToString(value));
			if (mustShowValue) text += $" of {value}";
			return text;
		}
	}
}
