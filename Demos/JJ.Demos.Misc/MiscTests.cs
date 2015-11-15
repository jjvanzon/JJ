using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using JJ.Framework.Testing;
using System.Globalization;

namespace JJ.Demos.Misc
{
    [TestClass]
    public class MiscTests
    {
        [TestMethod]
        public void Test_Linq_Take_MoreThanCollectionSize()
        {
            IList<int> source = new int[] { 1, 2, 3 };
            IList<int> dest = source.Take(5).ToArray();
            AssertHelper.AreEqual(3, () => dest.Count);
        }

        [TestMethod]
        public void Test_CultureInfo_TextInfo_ListSeparator_Chinese_Etc()
        {
            CultureInfo chineseCulture = new CultureInfo("zh-CN");
            CultureInfo russianCulture = new CultureInfo("ru-RU");
            CultureInfo dutchCulture = new CultureInfo("nl-NL");
            CultureInfo enUSCulture = new CultureInfo("en-US");

            // These are not exactly what you would expect in terms of punctuation, so in most cases there is no point using it.
            string chineseListSeparator = chineseCulture.TextInfo.ListSeparator;
            string russianListSeparator = russianCulture.TextInfo.ListSeparator;
            string dutchListSeparator = dutchCulture.TextInfo.ListSeparator;
            string enUSListSeparator = enUSCulture.TextInfo.ListSeparator;
        }
    }
}
