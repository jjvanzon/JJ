﻿using JJ.Framework.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable LocalNameCapturedOnly
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace JJ.Framework.Exceptions.Tests
{
	[TestClass]
	public class ExceptionTests
	{
		private class Item
		{
			public Item Parent { get; }
		}

		[TestMethod]
		public void Test_SimpleException_WithNameOf()
		{
			object value;
			AssertHelper.ThrowsException<NullException>(() => throw new NullException(nameof(value)), "value is null.");
		}

		[TestMethod]
		public void Test_SimpleException_WithExpression_WithSinglePart()
		{
			object value = null;
			AssertHelper.ThrowsException<NullException>(() => throw new NullException(() => value), "value is null.");
		}

		[TestMethod]
		public void Test_SimpleException_WithExpression_WithMultipleParts()
		{
			Item item = null;
			AssertHelper.ThrowsException<NullException>(() => throw new NullException(() => item.Parent), "item.Parent is null.");
		}

		[TestMethod]
		public void Test_ComparativeException_WithNameOf()
		{
			object value;
			AssertHelper.ThrowsException<LessThanException>(() => throw new LessThanException(nameof(value), 0), "value is less than 0.");
		}

		[TestMethod]
		public void Test_ComparativeException_WithExpression_WithSinglePart()
		{
			object value = null;
			AssertHelper.ThrowsException<LessThanException>(() => throw new LessThanException(() => value, 0), "value is less than 0.");
		}

		[TestMethod]
		public void Test_ComparativeException_WithExpression_WithMultipleParts()
		{
			Item item = null;
			AssertHelper.ThrowsException<LessThanException>(() => throw new LessThanException(() => item.Parent, 0), "item.Parent is less than 0.");
		}

		[TestMethod]
		public void Test_ComparativeException_WithLimitExpression()
		{
			Item item = null;
			double number = 0;

			AssertHelper.ThrowsException<LessThanException>(
				() => throw new LessThanException(() => item.Parent, () => number),
				"item.Parent is less than number of 0.");
		}
	}
}