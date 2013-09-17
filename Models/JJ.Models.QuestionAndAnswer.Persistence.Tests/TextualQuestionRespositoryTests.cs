using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Framework.Persistence;
using System.Collections.Generic;
using JJ.Models.QuestionAndAnswer.Persistence.Tests.Helpers;
using JJ.Framework.Configuration;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests
{
    [TestClass]
    public class TextualQuestionRespositoryTests
    {
        [TestMethod]
        public void Test_TextualQuestionRepository_Get()
        {
            foreach (string contextType in GetContextTypes())
            {
                using (IContext context = PersistenceHelper.CreatePersistenceContext(contextType))
                {
                    ITextualQuestionRepository repository = new TextualQuestionRepository(context, context.Location);
                    TextualQuestion item = repository.Get(2);
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

        private string[] GetContextTypes()
        {
            //return ConfigurationManager.GetSection<ConfigurationSection>().PersistenceContextTypes;
            return new string[] { ConfigurationManager.GetSection<PersistenceConfiguration>().ContextType };
        }
    }
}
