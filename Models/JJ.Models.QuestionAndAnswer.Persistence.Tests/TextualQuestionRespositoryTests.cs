using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Framework.Persistence;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests
{
    [TestClass]
    public class TextualQuestionRespositoryTests
    {
        [TestMethod]
        public void Test_TextualQuestionRepository_GetAll()
        {
            using (IContext context = PersistenceHelper.CreatePersistenceContext())
            {
                ITextualQuestionRepository repository = new TextualQuestionRepository(context);
                List<EntityWrapper<TextualQuestion>> list = repository.GetAll().ToList();
            }
        }

        [TestMethod]
        public void Test_TextualQuestionRepository_Get()
        {
            using (IContext context = PersistenceHelper.CreatePersistenceContext())
            {
                ITextualQuestionRepository repository = new TextualQuestionRepository(context);
                EntityWrapper<TextualQuestion> item = repository.Get(2);
            }
        }
    }
}
