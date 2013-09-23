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
    public class TextualQuestionRespositoryTests
    {
        private const int EXISTENT_TEXTUAL_QUESTION_ID = 12564;

        [TestMethod]
        public void Test_TextualQuestionRepository_Get()
        {
            foreach (string contextType in GetContextTypes())
            {
                using (IContext context = PersistenceHelper.CreatePersistenceContext(contextType))
                {
                    ITextualQuestionRepository repository = new TextualQuestionRepository(context, context.Location);
                    TextualQuestion item = repository.Get(EXISTENT_TEXTUAL_QUESTION_ID);
                }
            }
        }

        [TestMethod]
        public void Test_TextualQuestionRepository_GetAll()
        {
            foreach (string contextType in GetContextTypes())
            {
                using (IContext context = PersistenceHelper.CreatePersistenceContext(contextType))
                {
                    ITextualQuestionRepository repository = new TextualQuestionRepository(context, context.Location);
                    List<TextualQuestion> list = repository.GetAll().ToList();
                }
            }
        }

        [TestMethod]
        public void Test_TextualQuestionRepository_GetBySource()
        {
            foreach (string contextType in GetContextTypes())
            {
                using (IContext context = PersistenceHelper.CreatePersistenceContext(contextType))
                {
                    ITextualQuestionRepository repository = new TextualQuestionRepository(context, context.Location);
                    TextualQuestion[] list = repository.GetBySource((int)SourceEnum.Manual).ToArray();
                }
            }
        }

        private string[] GetContextTypes()
        {
            //return ConfigurationManager.GetSection<ConfigurationSection>().PersistenceContextTypes;
            return new string[] { ConfigurationManager.GetSection<PersistenceConfiguration>().ContextType };
        }
    }
}
