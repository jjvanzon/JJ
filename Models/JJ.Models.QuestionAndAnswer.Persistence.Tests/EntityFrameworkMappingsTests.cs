using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests
{
    [TestClass]
    public class EntityFrameworkMappingsTests
    {
        [TestMethod]
        public void Test_EntityFrameworkMapping()
        {
            string specialConnectionString = @"metadata=res://*/QuestionAndAnswer.csdl|res://*/QuestionAndAnswer.ssdl|res://*/QuestionAndAnswer.msl;provider=System.Data.SqlClient;provider connection string=""data source=.\SQLEXPRESS;initial catalog=QuestionAndAnswerDB_DEV;persist security info=True;user id=development;password=development;MultipleActiveResultSets=True;App=EntityFramework"";";
            using (DbContext context = new JJ.Models.QuestionAndAnswer.Persistence.EntityFramework.QuestionAndAnswerContext(specialConnectionString))
            {
                foreach (var entity in context.Set<TextualQuestion>())
                {
                    string value = entity.Text;
                }
            }
        }
    }
}
