//using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
//using JJ.Framework.Exceptions.Basic;
//using JJ.Presentation.QuestionAndAnswer.Helpers;
//using JJ.Presentation.QuestionAndAnswer.ToViewModel;
//using JJ.Presentation.QuestionAndAnswer.ViewModels;

//namespace JJ.Presentation.QuestionAndAnswer.Presenters
//{
//	public class QuestionNotFoundPresenter
//	{
//		private readonly string _authenticatedUserName;
//		private readonly IUserRepository _userRepository;

//		/// <param name="authenticatedUserName">nullable</param>
//		public QuestionNotFoundPresenter(IUserRepository userRepository, string authenticatedUserName)
//		{
//			_userRepository = userRepository ?? throw new NullException(() => userRepository);
//			_authenticatedUserName = authenticatedUserName;
//		}

//		public QuestionNotFoundViewModel Show()
//		{
//		    var viewModel = new QuestionNotFoundViewModel
//		    {
//		        Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _userRepository)
//		    };
//		    return viewModel;
//		}

//		public QuestionNotFoundViewModel SetLanguage(string cultureName)
//		{
//			CultureHelper.SetCulture(cultureName);
//			return Show();
//		}
//	}
//}
