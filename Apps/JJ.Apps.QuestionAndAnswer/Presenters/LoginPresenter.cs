using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Security;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class LoginPresenter
    {
        private IUserRepository _userRepository;

        public LoginPresenter(IUserRepository userRepository)
        {
            if (userRepository == null) { throw new ArgumentNullException("userRepository"); }
            _userRepository = userRepository;
        }

        public LoginViewModel Show()
        {
            return new LoginViewModel();
        }

        public LoginViewModel Login(LoginViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            User user = _userRepository.TryGetByUserName(viewModel.UserName);
            if (user == null)
            {
                viewModel.IsLoggedIn = false;
                viewModel.Password = null;
                return viewModel;
            }

            string passwordFromClient = viewModel.Password;
            string tokenFromClient = viewModel.SecurityToken;
            string passwordFromServer = user.Password;
            string saltFromServer = user.SecuritySalt;
            IAuthenticator authenticator = AuthenticationHelper.CreateAuthenticatorFromConfiguration();
            viewModel.IsLoggedIn = authenticator.IsAuthentic(passwordFromClient, tokenFromClient, passwordFromServer, saltFromServer);
            viewModel.Password = null;

            if (viewModel.IsLoggedIn)
            {
                viewModel.Name = user.Name;
            }

            return viewModel;
        }
    }
}
