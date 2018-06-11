using System.Data.Entity;
using System.Linq;
using JJ.Data.QuestionAndAnswer.EntityFramework.Mapping;
using JJ.Data.QuestionAndAnswer.Tests.Helpers;
using JJ.Framework.Configuration;
using JJ.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace JJ.Data.QuestionAndAnswer.Tests
{
    [TestClass]
    public class QuestionAndAnswerOrmMappingsTests
    {
        [TestMethod]
        public void Test_QuestionAndAnswerOrmMappings_EntityFramework_Directly()
        {
            var specialConnectionString =
                @"metadata=res://*/Mapping.QuestionAndAnswer.csdl|res://*/Mapping.QuestionAndAnswer.ssdl|res://*/Mapping.QuestionAndAnswer.msl;provider=System.Data.SqlClient;provider connection string=""data source=.\SQLEXPRESS;initial catalog=DEV_QuestionAndAnswerDB_UnitTests;persist security info=True;user id=dev;password=dev;MultipleActiveResultSets=True;App=EntityFramework"";";

            using (DbContext context = new QuestionAndAnswerContext(specialConnectionString))
            {
                foreach (Question entity in context.Set<Question>())
                {
                    string value = entity.Text;
                }
            }
        }

        [TestMethod]
        public void Test_QuestionAndAnswerOrmMappings_EntityFramework_UsingIContext()
        {
            using (IContext context = CreateEntityFrameworkContext())
            {
                int existingQuestionID = GetExistingQuestionID();
                var question = context.Get<Question>(existingQuestionID);
            }
        }

        [TestMethod]
        public void Test_QuestionAndAnswerOrmMappings_NHibernate_UsingIContext()
        {
            using (IContext context = CreateNHibernateContext())
            {
                int existingQuestionID = GetExistingQuestionID();
                var question = context.Get<Question>(existingQuestionID);
            }
        }

        private IContext CreateNHibernateContext()
        {
            PersistenceConfiguration persistenceConfiguration = GetNHibernatePersistenceConfiguration();
            return ContextFactory.CreateContextFromConfiguration(persistenceConfiguration);
        }

        private PersistenceConfiguration GetNHibernatePersistenceConfiguration()
        {
            const string contextTypeName = "NHibernate";

            return CustomConfigurationManager.GetSection<ConfigurationSection>()
                                             .PersistenceConfigurations
                                             .Single(x => x.ContextType == contextTypeName);
        }

        private IContext CreateEntityFrameworkContext()
        {
            PersistenceConfiguration persistenceConfiguration = GetEntityFrameworkPersistenceConfiguration();
            return ContextFactory.CreateContextFromConfiguration(persistenceConfiguration);
        }

        private PersistenceConfiguration GetEntityFrameworkPersistenceConfiguration()
        {
            var contextTypeName = "EntityFramework";

            return CustomConfigurationManager.GetSection<ConfigurationSection>()
                                             .PersistenceConfigurations.Where(x => x.ContextType == contextTypeName)
                                             .Single();
        }

        private int GetExistingQuestionID()
        {
            int existingQuestionID = CustomConfigurationManager.GetSection<ConfigurationSection>().ExistingQuestionID;
            return existingQuestionID;
        }
    }
}