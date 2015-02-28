﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Framework.Reflection.Tests.ExpressionHelperTestHelpers;

namespace JJ.Framework.Reflection.Tests
{
    [TestClass]
    public class ExpressionHelperGetTextSimpleTests
    {
        // There are separate test classes for the simple tests,
        // because in the past these tests were used to test multiple
        // candidate implementations of ExpressionHelper
        // that did not support the full set of features.

        [TestMethod]
        public void Test_ExpressionHelpers_GetText_LocalVariable()
        {
            int variable = 1;
            Assert.AreEqual("variable", ExpressionHelper.GetText(() => variable));
        }

        [TestMethod]
        public void Test_ExpressionHelpers_GetText_Field()
        {
            Item item = new Item { Field = 1 };
            Assert.AreEqual("item.Field", ExpressionHelper.GetText(() => item.Field));
        }

        [TestMethod]
        public void Test_ExpressionHelpers_GetText_Property()
        {
            Item item = new Item { Property = 1 };
            Assert.AreEqual("item.Property", ExpressionHelper.GetText(() => item.Property));
        }

        [TestMethod]
        public void Test_ExpressionHelpers_GetText_ArrayLength()
        {
            Item[] items = { null, null, null };
            Assert.AreEqual("items.Length", ExpressionHelper.GetText(() => items.Length));
        }

        [TestMethod]
        public void Test_ExpressionHelpers_GetText_WithQualifier()
        {
            Item grandParentItem = new Item { Index = 10 };
            Item parentItem = new Item { Parent = grandParentItem };
            Item item = new Item { Parent = parentItem };

            Assert.AreEqual("item.Parent.Parent.Index", ExpressionHelper.GetText(() => item.Parent.Parent.Index));
        }
    }
}