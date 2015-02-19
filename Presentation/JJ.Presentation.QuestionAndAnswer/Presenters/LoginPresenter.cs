using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection;
using JJ.Framework.Security;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class LoginPresenter
    {
        private static ActionDescriptor _defaultReturnAction;

        private Repositories _repositories;

        static LoginPresenter()
        {
            _defaultReturnAction = ActionDescriptorHelper.CreateActionDescriptor<RandomQuestionPresenter>(x => x.Show(null));
        }

        public LoginPresenter(Repositories repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);
            _repositories = repositories;
        }

        public LoginViewModel Show()
        {
            LoginViewModel viewModel = ViewModelHelper.CreateLoginViewModel();
            viewModel.ReturnAction = _defaultReturnAction;
            return viewModel;
        }

        public LoginViewModel Show(ActionDescriptor sourceAction)
        {
            if (sourceAction == null) throw new NullException(() => sourceAction);

            LoginViewModel viewModel = ViewModelHelper.CreateLoginViewModel();
            viewModel.ReturnAction = sourceAction;
            return viewModel;
        }
        
        public object Login(LoginViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

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
                    object viewModel2 = ActionDispatcher.GetViewModel(viewModel.ReturnAction, _repositories, viewModel.UserName);
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
