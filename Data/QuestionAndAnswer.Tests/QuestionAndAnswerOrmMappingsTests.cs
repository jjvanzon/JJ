using JJ.Framework.Configuration;
using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace JJ.Data.QuestionAndAnswer.Tests
{
	[TestClass]
	public class QuestionAndAnswerOrmMappingsTests
	{
		[TestMethod]
		public void Test_QuestionAndAnswerOrmMappings_EntityFramework5_Directly()
		{
			string specialConnectionString = @"metadata=res://*/Mapping.QuestionAndAnswer.csdl|res://*/Mapping.QuestionAndAnswer.ssdl|res://*/Mapping.QuestionAndAnswer.msl;provider=System.Data.SqlClient;provider connection string=""data source=.\SQLEXPRESS;initial catalog=DEV_QuestionAndAnswerDB_UnitTests;persist security info=True;user id=dev;password=enteentest0!;MultipleActiveResultSets=True;App=EntityFramework"";";
			using (DbContext context = new JJ.Data.QuestionAndAnswer.EntityFramework5.Mapping.QuestionAndAnswerContext(specialConnectionString))
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
