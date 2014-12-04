using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
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

        /// <summary>
        /// Can return LoginViewModel or SmallLoginViewModel.
        /// </summary>
        public LoginViewModel Login(string password, string securityToken, LoginViewModel viewModel)
        {
            string userName = viewModel.UserName;

            var viewModel2 = new LoginViewModel();
            viewModel2.UserName = userName;

            User user = _userRepository.TryGetByUserName(userName);
            if (user != null)
            {
                string passwordFromClient = password;
                string tokenFromClient = securityToken;
                string passwordFromServer = user.Password;
                string saltFromServer = user.SecuritySalt;
                IAuthenticator authenticator = AuthenticationHelper.CreateAuthenticatorFromConfiguration();
                bool isAuthentic = authenticator.IsAuthentic(passwordFromClient, tokenFromClient, passwordFromServer, saltFromServer);

                if (isAuthentic)
                {
                    viewModel2.IsAuthenticated = true;
                }
            }

            return viewModel2;
        }
    }
}
