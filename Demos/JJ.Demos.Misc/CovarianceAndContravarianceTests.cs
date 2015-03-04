using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JJ.Demos.Misc
{
    [TestClass]
    public class CovarianceAndContravarianceTests
    {
        private class Base
        { }

        private class DerivedClass : Base
        { }

        [TestMethod]
        public void Test_ContraVariance_Func()
        {
            Func<object, object> baseFunc = b => b;
            Func<DerivedClass, object> derivedFunc = d => d;

            //baseFunc = derivedFunc; // Does not compile.
            derivedFunc = baseFunc;
        }
    }
}
