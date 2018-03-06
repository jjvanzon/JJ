﻿using System;
using System.Linq.Expressions;

namespace JJ.Framework.Exceptions.Aggregates
{
	public class NotUniqueException : ExceptionWithNameTypeAndKeyBase
	{
		protected override string MessageWithName => "{0} not unique.";
		protected override string MessageWithNameAndKey => "{0} with key {1} not unique.";

		/// <inheritdoc />
		public NotUniqueException(Expression<Func<object>> expression) : base(expression) { }

		/// <inheritdoc />
		public NotUniqueException(Expression<Func<object>> expression, object key) : base(expression, key) { }

		/// <inheritdoc />
		public NotUniqueException(Type type) : base(type) { }

		/// <inheritdoc />
		public NotUniqueException(Type type, object key) : base(type, key) { }

		/// <inheritdoc />
		public NotUniqueException(string typeName) : base(typeName) { }

		/// <inheritdoc />
		public NotUniqueException(string typeName, object key) : base(typeName, key) { }
	}
}