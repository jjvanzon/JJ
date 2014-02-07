using JJ.Framework.Configuration;
using JJ.Framework.Persistence;
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
    public class QuestionAndAnswerOrmMappingsTests
    {
        [TestMethod]
        public void Test_OrmMapping_EntityFramework5_Directly()
        {
            string specialConnectionString = @"metadata=res://*/QuestionAndAnswer.csdl|res://*/QuestionAndAnswer.ssdl|res://*/QuestionAndAnswer.msl;provider=System.Data.SqlClient;provider connection string=""data source=.\SQLEXPRESS;initial catalog=QuestionAndAnswerDB_UnitTests_DEV;persist security info=True;user id=development;password=development;MultipleActiveResultSets=True;App=EntityFramework"";";
            using (DbContext context = new JJ.Models.QuestionAndAnswer.Persistence.EntityFramework5.QuestionAndAnswerContext(specialConnectionString))
            {
                foreach (var entity in context.Set<Question>())
                {
                    string value = entity.Text;
                }
            }
        }

        [TestMethod]
        public void Test_OrmMapping_EntityFramework5_UsingIContext()
        {
            PersistenceConfiguration config = CustomConfigurationManager.GetSection<PersistenceConfiguration>();

            int existingQuestionID = AppSettings<IAppSettings>.Get(x => x.ExistingQuestionID);

            string persistenceTypeName = "JJ.Framework.Persistence.EntityFramework5";

            using (IContext context = ContextFactory.CreateContext(persistenceTypeName, config.Location, config.ModelAssembly, config.MappingAssembly))
            {
                Question question = context.Get<Question>(existingQuestionID);
            }
        }

        [TestMethod]
        public void Test_OrmMapping_NHibernate_UsingIContext()
        {
            PersistenceConfiguration config = CustomConfigurationManager.GetSection<PersistenceConfiguration>();

            int existingQuestionID = AppSettings<IAppSettings>.Get(x => x.ExistingQuestionID);

            string persistenceTypeName = "JJ.Framework.Persistence.NHibernate";

            using (IContext context = ContextFactory.CreateContext(persistenceTypeName, config.Location, config.ModelAssembly, config.MappingAssembly))
            {
                Question question = context.Get<Question>(existingQuestionID);
            }
        }
    }
}
