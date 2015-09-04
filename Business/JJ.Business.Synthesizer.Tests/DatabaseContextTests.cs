﻿using JJ.Business.Synthesizer.Tests.Helpers;
using JJ.Framework.Data;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class DatabaseContextTests
    {
        [TestMethod]
        public void Test_Synthesizer_DatabaseContext()
        {
            using (IContext context = PersistenceHelper.CreateDatabaseContext())
            {
                var operatorRepository = PersistenceHelper.CreateRepository<IOperatorRepository>(context);
                var op = new Operator();
                op.Name = "Test Operator";
                operatorRepository.Insert(op);
            }
        }
    }
}
