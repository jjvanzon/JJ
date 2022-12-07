//using JJ.Framework.Presentation;
//using JJ.Framework.Exceptions.Basic;

//namespace JJ.Presentation.QuestionAndAnswer.Helpers
//{
//	internal static class DispatchHelper
//	{
//		public static object DispatchAction(ActionInfo actionInfo, Repositories repositories, string authenticatedUserName)
//		{
//			if (actionInfo == null) throw new NullException(() => actionInfo);
//			if (repositories == null) throw new NullException(() => repositories);

//			var presenterConstructorArguments = new
//			{
//				repositories,
//				authenticatedUserName,
//				categoryRepository = repositories.CategoryRepository,
//				questionRepository = repositories.QuestionRepository,
//				questionFlagRepository = repositories.QuestionFlagRepository,
//				flagStatusRepository = repositories.FlagStatusRepository,
//				userRepository = repositories.UserRepository
//			};

//			object viewModel = ActionDispatcher.Dispatch(actionInfo, presenterConstructorArguments);
//			return viewModel;
//		}
//	}
//}
