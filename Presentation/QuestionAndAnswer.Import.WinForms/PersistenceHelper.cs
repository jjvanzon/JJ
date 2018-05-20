using JJ.Framework.Data;

namespace JJ.Presentation.QuestionAndAnswer.Import.WinForms
{
	internal static class PersistenceHelper
	{
		public static IContext CreateContext() => ContextFactory.CreateContextFromConfiguration();

		public static TRepositoryInterface CreateRepository<TRepositoryInterface>(IContext context)
			=> RepositoryFactory.CreateRepositoryFromConfiguration<TRepositoryInterface>(context);
	}
}