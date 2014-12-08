using JJ.Framework.Configuration;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Tests
{
    [TestClass]
    public class QuestionAndAnswerOrmMappingsTests
    {
        [TestMethod]
        public void Test_QuestionAndAnswerOrmMappings_EntityFramework5_Directly()
        {
            string specialConnectionString = @"metadata=res://*/QuestionAndAnswer.csdl|res://*/QuestionAndAnswer.ssdl|res://*/QuestionAndAnswer.msl;provider=System.Data.SqlClient;provider connection string=""data source=.\SQLEXPRESS;initial catalog=QuestionAndAnswerDB_UnitTests_DEV;persist security info=True;user id=development;password=development;MultipleActiveResultSets=True;App=EntityFramework"";";
            using (DbContext context = new JJ.Models.QuestionAndAnswer.EntityFramework5.QuestionAndAnswerContext(specialConnectionString))
            {
                foreach (var entity in context.Set<Question>())
                {
                    string value = entity.Text;
                }
            }
        }

        [TestMethod]
        public void Test_QuestionAndAnswerOrmMappings_EntityFramework5_UsingIContext()
        {
            using (IContext context = CreateEntityFramework5Context())
            {
                int existingQuestionID = GetExistingQuestionID();
                Question question = context.Get<Question>(existingQuestionID);
            }
        }

        [TestMethod]
        public void Test_QuestionAndAnswerOrmMappings_NHibernate_UsingIContext()
        {
            using (IContext context = CreateNHibernateContext())
            {
                int existingQuestionID = GetExistingQuestionID();
                Question question = context.Get<Question>(existingQuestionID);
            }
        }

        private IContext CreateNHibernateContext()
        {
            PersistenceConfiguration persistenceConfiguration = GetNHibernatePersistenceConfiguration();
            return ContextFactory.CreateContextFromConfiguration(persistenceConfiguration);
        }

        private PersistenceConfiguration GetNHibernatePersistenceConfiguration()
        {
            string contextTypeName = "NHibernate";
            return CustomConfigurationManager.GetSection<ConfigurationSection>().PersistenceConfigurations.Where(x => x.ContextType == contextTypeName).Single();
        }

        private IContext CreateEntityFramework5Context()
        {
            PersistenceConfiguration persistenceConfiguration = GetEntityFramework5PersistenceConfiguration();
            return ContextFactory.CreateContextFromConfiguration(persistenceConfiguration);
        }

        private PersistenceConfiguration GetEntityFramework5PersistenceConfiguration()
        {
            string contextTypeName = "EntityFramework5";
            return CustomConfigurationManager.GetSection<ConfigurationSection>().PersistenceConfigurations.Where(x => x.ContextType == contextTypeName).Single();
        }

        private int GetExistingQuestionID()
        {
            int existingQuestionID = CustomConfigurationManager.GetSection<ConfigurationSection>().ExistingQuestionID;
            return existingQuestionID;
        }
    }
}
