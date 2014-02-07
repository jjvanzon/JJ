using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Framework.Persistence;
using System.Collections.Generic;
using JJ.Models.QuestionAndAnswer.Persistence.Tests.Helpers;
using JJ.Framework.Configuration;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Business.QuestionAndAnswer.Enums;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests
{
    [TestClass]
    public class QuestionRespositoryTests
    {
        [TestMethod]
        public void Test_QuestionRepository_Get()
        {
            foreach (string contextType in GetContextTypes())
            {
                using (IContext context = ContextHelper.CreateContext(contextType))
                {
                    int id = AppSettings<IAppSettings>.Get(x => x.ExistingQuestionID);
                    IQuestionRepository repository = new QuestionRepository(context, context.Location);
                    Question item = repository.Get(id);
                }
            }
        }

        [TestMethod]
        public void Test_QuestionRepository_GetAll()
        {
            foreach (string contextType in GetContextTypes())
            {
                using (IContext context = ContextHelper.CreateContext(contextType))
                {
                    IQuestionRepository repository = new QuestionRepository(context, context.Location);
                    List<Question> list = repository.GetAll().ToList();
                }
            }
        }

        private const int TEST_SOURCE_ID = 1;

        [TestMethod]
        public void Test_QuestionRepository_GetBySource()
        {
            foreach (string contextType in GetContextTypes())
            {
                using (IContext context = ContextHelper.CreateContext(contextType))
                {
                    IQuestionRepository repository = new QuestionRepository(context, context.Location);
                    Question[] list = repository.GetBySourceID(TEST_SOURCE_ID).ToArray();
                }
            }
        }

        private string[] GetContextTypes()
        {
            //return CustomConfigurationManager.GetSection<ConfigurationSection>();

            return new string[] { CustomConfigurationManager.GetSection<PersistenceConfiguration>().ContextType };
        }
    }
}
