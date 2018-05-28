using System.Collections.Generic;
using System.Linq;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Data.QuestionAndAnswer.Tests.Helpers;
using JJ.Framework.Configuration;
using JJ.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable UnusedVariable

namespace JJ.Data.QuestionAndAnswer.Tests
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
					var repository = RepositoryFactory.CreateRepositoryFromConfiguration<IQuestionRepository>(context, persistenceConfiguration);
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
					var repository = RepositoryFactory.CreateRepositoryFromConfiguration<IQuestionRepository>(context, persistenceConfiguration);
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
					var repository = RepositoryFactory.CreateRepositoryFromConfiguration<IQuestionRepository>(context, persistenceConfiguration);
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
