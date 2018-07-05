using System.Security.Authentication;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Security;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
// ReSharper disable MemberCanBeMadeStatic.Global

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class LoginPresenter
	{
		private readonly Repositories _repositories;

	    public LoginPresenter(Repositories repositories) => _repositories = repositories ?? throw new NullException(() => repositories);

		public LoginViewModel Show()
		{
			LoginViewModel viewModel = ViewModelHelper.CreateLoginViewModel();
			return viewModel;
		}
		
		public LoginViewModel Login(LoginViewModel userInput)
		{
			if (userInput == null) throw new NullException(() => userInput);

			User user = _repositories.UserRepository.TryGetByUserName(userInput.UserName);
			if (user != null)
			{
				string passwordFromClient = userInput.Password;
				string tokenFromClient = userInput.SecurityToken;
				string passwordFromServer = user.Password;
				string saltFromServer = user.SecuritySalt;
				IAuthenticator authenticator = AuthenticationHelper.CreateAuthenticatorFromConfiguration();
				bool isAuthenticated = authenticator.IsAuthentic(passwordFromClient, tokenFromClient, passwordFromServer, saltFromServer);

				if (isAuthenticated)
				{
			        LoginViewModel viewModel = ViewModelHelper.CreateLoginViewModel();
				    viewModel.IsAuthenticated = true;
                    return viewModel;
				}
			}

            throw new AuthenticationException();
		}

		public LoginViewModel SetLanguage(LoginViewModel userInput, string cultureName)
		{
			if (userInput == null) throw new NullException(() => userInput);

			CultureHelper.SetCulture(cultureName);

			LoginViewModel viewModel = ViewModelHelper.CreateLoginViewModel();
			viewModel.UserName = userInput.UserName;
			return viewModel;
		}
	}
}
