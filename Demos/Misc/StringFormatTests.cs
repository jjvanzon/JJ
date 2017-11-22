﻿using System;
using JJ.Framework.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JJ.Demos.Misc
{
	[TestClass]
	public class StringFormatTests
	{
		[TestMethod]
		public void Test_StringFormat_TooManyPlaceHolders_ThrowsException()
		{
			AssertHelper.ThrowsException<FormatException>(() => string.Format("{0} {1}", "bla"));

		}

		[TestMethod]
		public void Test_StringFormat_NotEnoughPlaceHolders_NoException()
		{
			string str = string.Format("{0}", "bla", "bla2");

		}

		[TestMethod]
		public void Test_StringFormat_EmptyString_ButPlaceHolderReplacements_NoException()
		{
			string str = string.Format("", "bla");
		}

		[TestMethod]
		public void Test_StringFormat_PreciselyEnoughPlaceHolders()
		{
			string str = string.Format("{0}", "bla");

		}

		[TestMethod]
		public void Test_StringFormat_BadlyFormattedPlaceholders_MissingClosingBrace_ThrowsException()
		{
			AssertHelper.ThrowsException<FormatException>(() => string.Format("{0", "bla"));
		}

		[TestMethod]
		public void Test_StringFormat_BadlyFormattedPlaceholders_SpacesInsideBrace_ThrowsException()
		{
			AssertHelper.ThrowsException<FormatException>(() => string.Format("{ 0}", "bla"));
		}

		[TestMethod]
		public void Test_InterpolatedString_WithEmbeddedFormatString_NoException()
		{
			string formatString = "FormatString {0} {1}";
			string formattedFormatString = $"InterpolatedString {formatString}";
		}
	}
}
