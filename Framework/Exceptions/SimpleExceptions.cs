﻿using System;
using System.Linq.Expressions;

namespace JJ.Framework.Exceptions
{
		/// <inheritdoc />
		public class NullException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is null.";

			/// <inheritdoc />
			public NullException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NullException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class CollectionEmptyException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} collection is empty.";

			/// <inheritdoc />
			public CollectionEmptyException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public CollectionEmptyException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class CollectionNotEmptyException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} collection should be empty.";

			/// <inheritdoc />
			public CollectionNotEmptyException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public CollectionNotEmptyException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class HasNullsException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} contains nulls.";

			/// <inheritdoc />
			public HasNullsException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public HasNullsException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class HasValueException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should not have a value.";

			/// <inheritdoc />
			public HasValueException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public HasValueException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class InfinityException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is Infinity.";

			/// <inheritdoc />
			public InfinityException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public InfinityException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class InvalidReferenceException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} not found in list.";

			/// <inheritdoc />
			public InvalidReferenceException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public InvalidReferenceException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class IsDateTimeException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should not be a DateTime.";

			/// <inheritdoc />
			public IsDateTimeException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public IsDateTimeException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class IsDecimalException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should not be a Decimal.";

			/// <inheritdoc />
			public IsDecimalException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public IsDecimalException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class IsDoubleException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should not be a double precision floating point number.";

			/// <inheritdoc />
			public IsDoubleException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public IsDoubleException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class IsIntegerException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should not be an integer number.";

			/// <inheritdoc />
			public IsIntegerException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public IsIntegerException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NaNException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is NaN.";

			/// <inheritdoc />
			public NaNException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NaNException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotDateTimeException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is not a DateTime.";

			/// <inheritdoc />
			public NotDateTimeException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotDateTimeException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotDecimalException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is not a Decimal.";

			/// <inheritdoc />
			public NotDecimalException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotDecimalException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotDoubleException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is not a double precision floating point number.";

			/// <inheritdoc />
			public NotDoubleException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotDoubleException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotHasValueException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} has no value.";

			/// <inheritdoc />
			public NotHasValueException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotHasValueException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotInfinityException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should be Infinity.";

			/// <inheritdoc />
			public NotInfinityException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotInfinityException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotIntegerException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is not an integer number.";

			/// <inheritdoc />
			public NotIntegerException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotIntegerException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotNaNException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should be NaN.";

			/// <inheritdoc />
			public NotNaNException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotNaNException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotNullException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should be null.";

			/// <inheritdoc />
			public NotNullException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotNullException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotNullOrEmptyException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should be null or empty.";

			/// <inheritdoc />
			public NotNullOrEmptyException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotNullOrEmptyException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NotNullOrWhiteSpaceException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} should be null or white space.";

			/// <inheritdoc />
			public NotNullOrWhiteSpaceException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NotNullOrWhiteSpaceException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NullOrEmptyException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is null or empty.";

			/// <inheritdoc />
			public NullOrEmptyException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NullOrEmptyException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class NullOrWhiteSpaceException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is null or white space.";

			/// <inheritdoc />
			public NullOrWhiteSpaceException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public NullOrWhiteSpaceException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}

		/// <inheritdoc />
		public class ZeroException : SimpleExceptionBase
		{
			private const string MESSAGE_TEMPLATE = "{0} is 0.";

			/// <inheritdoc />
			public ZeroException(Expression<Func<object>> expression) : base(MESSAGE_TEMPLATE, expression) { }

			/// <inheritdoc />
			public ZeroException(string name) : base(MESSAGE_TEMPLATE, name) { }
		}


}
