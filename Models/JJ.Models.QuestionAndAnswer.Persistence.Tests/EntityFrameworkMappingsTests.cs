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
    public class EntityFramework5MappingsTests
    {
        [TestMethod]
        public void Test_EntityFramework5Mapping()
        {
            string specialConnectionString = @"metadata=res://*/QuestionAndAnswer.csdl|res://*/QuestionAndAnswer.ssdl|res://*/QuestionAndAnswer.msl;provider=System.Data.SqlClient;provider connection string=""data source=.\SQLEXPRESS;initial catalog=QuestionAndAnswerDB_DEV;persist security info=True;user id=development;password=development;MultipleActiveResultSets=True;App=EntityFramework"";";
            using (DbContext context = new JJ.Models.QuestionAndAnswer.Persistence.EntityFramework5.QuestionAndAnswerContext(specialConnectionString))
            {
                foreach (var entity in context.Set<Question>())
                {
                    string value = entity.Text;
                }
            }
        }
    }
}
