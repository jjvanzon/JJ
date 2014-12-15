using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Framework.Persistence;
using System.Collections.Generic;
using JJ.Models.QuestionAndAnswer.Tests.Helpers;
using JJ.Framework.Configuration;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Models.QuestionAndAnswer.Repositories;
using JJ.Business.QuestionAndAnswer.Enums;

namespace JJ.Models.QuestionAndAnswer.Tests
{
    [TestClass]
    public class QuestionRespositoryTests
    {
        [TestMethod]
        public void Test_QuestionRepository_Get()
        {
            foreach (PersistenceConfiguration persistenceConfiguration in GetPersistenceConfigurations())
            {
                using (IContext context = ContextFactory.CreateContextFromConfiguration(persistenceConfiguration))
                {
                    int id = GetExistingQuestionID();
                    IQuestionRepository repository = new QuestionRepository(context);
                    Question item = repository.Get(id);
                }
            }
        }

        [TestMethod]
        public void Test_QuestionRepository_GetAll()
        {
            foreach (PersistenceConfiguration persistenceConfiguration in GetPersistenceConfigurations())
            {
                using (IContext context = ContextFactory.CreateContextFromConfiguration(persistenceConfiguration))
                {
                    IQuestionRepository repository = new QuestionRepository(context);
                    List<Question> list = repository.GetAll().ToList();
                }
            }
        }

        private const int TEST_SOURCE_ID = 1;

        [TestMethod]
        public void Test_QuestionRepository_GetBySource()
        {
            foreach (PersistenceConfiguration persistenceConfiguration in GetPersistenceConfigurations())
            {
                using (IContext context = ContextFactory.CreateContextFromConfiguration(persistenceConfiguration))
                {
                    IQuestionRepository repository = new QuestionRepository(context);
                    Question[] list = repository.GetBySourceID(TEST_SOURCE_ID).ToArray();
                }
            }
        }

        private PersistenceConfiguration[] GetPersistenceConfigurations()
        {
            return CustomConfigurationManager.GetSection<ConfigurationSection>().PersistenceConfigurations;
        }

        private int GetExistingQuestionID()
        {
            int existingQuestionID = CustomConfigurationManager.GetSection<ConfigurationSection>().ExistingQuestionID;
            return existingQuestionID;
        }
    }
}
