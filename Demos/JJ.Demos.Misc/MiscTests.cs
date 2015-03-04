using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using JJ.Framework.Testing;

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
    }
}
