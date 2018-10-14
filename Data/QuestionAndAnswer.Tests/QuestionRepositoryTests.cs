using System.Collections.Generic;
using System.Linq;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Data.QuestionAndAnswer.Tests.Helpers;
using JJ.Framework.Configuration;
using JJ.Framework.Data;
using JJ.Framework.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace JJ.Data.QuestionAndAnswer.Tests
{
    [TestClass]
    public class QuestionRepositoryTests
    {
        [TestMethod]
        public void Test_QuestionRepository_Get()
            => AssertInconclusiveHelper.WithConnectionInconclusiveAssertion(
                () =>
                {
                    foreach (PersistenceConfiguration persistenceConfiguration in GetPersistenceConfigurations())
                    {
                        using (IContext context = ContextFactory.CreateContextFromConfiguration(persistenceConfiguration))
                        {
                            int id = GetExistingQuestionID();

                            var repository =
                                RepositoryFactory.CreateRepositoryFromConfiguration<IQuestionRepository>(context, persistenceConfiguration);

                            Question item = repository.Get(id);
                        }
                    }
                });

        [TestMethod]
        public void Test_QuestionRepository_GetAll()
            => AssertInconclusiveHelper.WithConnectionInconclusiveAssertion(
                () =>
                {
                    foreach (PersistenceConfiguration persistenceConfiguration in GetPersistenceConfigurations())
                    {
                        using (IContext context = ContextFactory.CreateContextFromConfiguration(persistenceConfiguration))
                        {
                            var repository =
                                RepositoryFactory.CreateRepositoryFromConfiguration<IQuestionRepository>(context, persistenceConfiguration);

                            List<Question> list = repository.GetAll().ToList();
                        }
                    }
                });

        private const int TEST_SOURCE_ID = 1;

        [TestMethod]
        public void Test_QuestionRepository_GetBySource()
            => AssertInconclusiveHelper.WithConnectionInconclusiveAssertion(
                () =>
                {
                    foreach (PersistenceConfiguration persistenceConfiguration in GetPersistenceConfigurations())
                    {
                        using (IContext context = ContextFactory.CreateContextFromConfiguration(persistenceConfiguration))
                        {
                            var repository =
                                RepositoryFactory.CreateRepositoryFromConfiguration<IQuestionRepository>(context, persistenceConfiguration);

                            Question[] list = repository.GetBySourceID(TEST_SOURCE_ID).ToArray();
                        }
                    }
                });

        private PersistenceConfiguration[] GetPersistenceConfigurations()
            => CustomConfigurationManager.GetSection<ConfigurationSection>().PersistenceConfigurations;

        private int GetExistingQuestionID()
        {
            int existingQuestionID = CustomConfigurationManager.GetSection<ConfigurationSection>().ExistingQuestionID;
            return existingQuestionID;
        }
    }
}