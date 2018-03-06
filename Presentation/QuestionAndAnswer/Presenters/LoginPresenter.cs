using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Framework.Presentation;
using JJ.Framework.Exceptions;
using JJ.Framework.Security;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class LoginPresenter
	{
		private static ActionInfo _defaultReturnAction;

		private Repositories _repositories;

		static LoginPresenter()
		{
			_defaultReturnAction = ActionDispatcher.CreateActionInfo<RandomQuestionPresenter>(x => x.Show(null));
		}

		public LoginPresenter(Repositories repositories)
		{
			if (repositories == null) throw new NullException(() => repositories);
			_repositories = repositories;
		}

		public LoginViewModel Show(ActionInfo returnAction = null)
		{
			LoginViewModel viewModel = ViewModelHelper.CreateLoginViewModel();
			viewModel.ReturnAction = returnAction ?? _defaultReturnAction;
			return viewModel;
		}
		
		public object Login(LoginViewModel viewModel)
		{
			if (viewModel == null) throw new NullException(() => viewModel);

			viewModel.ReturnAction = viewModel.ReturnAction ?? _defaultReturnAction;

			User user = _repositories.UserRepository.TryGetByUserName(viewModel.UserName);
			if (user != null)
			{
				string passwordFromClient = viewModel.Password;
				string tokenFromClient = viewModel.SecurityToken;
				string passwordFromServer = user.Password;
				string saltFromServer = user.SecuritySalt;
				IAuthenticator authenticator = AuthenticationHelper.CreateAuthenticatorFromConfiguration();
				bool isAuthentic = authenticator.IsAuthentic(passwordFromClient, tokenFromClient, passwordFromServer, saltFromServer);

				if (isAuthentic)
				{
					object viewModel2 = DispatchHelper.DispatchAction(viewModel.ReturnAction, _repositories, viewModel.UserName);
					return viewModel2;
				}
			}

			LoginViewModel loginViewModel = ViewModelHelper.CreateLoginViewModel();
			loginViewModel.ReturnAction = viewModel.ReturnAction;
			loginViewModel.UserName = viewModel.UserName;
			return loginViewModel;
		}

		public LoginViewModel SetLanguage(LoginViewModel viewModel, string cultureName)
		{
			if (viewModel == null) throw new NullException(() => viewModel);

			CultureHelper.SetCulture(cultureName);

			LoginViewModel viewModel2 = ViewModelHelper.CreateLoginViewModel();
			viewModel2.UserName = viewModel.UserName;
			viewModel2.ReturnAction = viewModel.ReturnAction;
			return viewModel2;
		}
	}
}
