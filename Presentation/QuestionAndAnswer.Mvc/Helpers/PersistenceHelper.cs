using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;
using JJ.Presentation.QuestionAndAnswer.Helpers;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Helpers
{
	internal static class PersistenceHelper
	{
		public static IContext CreateContext() => ContextFactory.CreateContextFromConfiguration();

		public static TRepositoryInterface CreateRepository<TRepositoryInterface>(IContext context)
			=> RepositoryFactory.CreateRepositoryFromConfiguration<TRepositoryInterface>(context);

		public static Repositories CreateRepositories(IContext context)
			=> new Repositories(
				CreateRepository<IQuestionRepository>(context),
				CreateRepository<IAnswerRepository>(context),
				CreateRepository<ICategoryRepository>(context),
				CreateRepository<IQuestionCategoryRepository>(context),
				CreateRepository<IQuestionLinkRepository>(context),
				CreateRepository<IQuestionFlagRepository>(context),
				CreateRepository<IFlagStatusRepository>(context),
				CreateRepository<ISourceRepository>(context),
				CreateRepository<IQuestionTypeRepository>(context),
				CreateRepository<IUserRepository>(context));
	}
}