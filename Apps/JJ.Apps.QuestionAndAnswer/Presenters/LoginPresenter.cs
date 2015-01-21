using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection;
using JJ.Framework.Security;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class LoginPresenter
    {
        private static ActionDescriptor _defaultSourceAction;

        private Repositories _repositories;

        static LoginPresenter()
        {
            _defaultSourceAction = ActionDescriptorHelper.CreateActionDescriptor<RandomQuestionPresenter>(x => x.Show(null));
        }

        public LoginPresenter(Repositories repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);
            _repositories = repositories;
        }

        public LoginViewModel Show()
        {
            return Show(_defaultSourceAction);
        }

        public LoginViewModel Show(ActionDescriptor sourceAction)
        {
            LoginViewModel viewModel = ViewModelHelper.CreateLoginViewModel();
            viewModel.SourceAction = sourceAction;
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
                    object viewModel2 = ActionDispatcher.GetViewModel(viewModel.SourceAction, _repositories, viewModel.UserName);
                    return viewModel2;
                }
            }

            LoginViewModel loginViewModel = ViewModelHelper.CreateLoginViewModel();
            loginViewModel.SourceAction = viewModel.SourceAction;
            loginViewModel.UserName = viewModel.UserName;
            return loginViewModel;
        }

        public LoginViewModel SetLanguage(LoginViewModel viewModel, string cultureName)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            CultureHelper.SetCulture(cultureName);

            LoginViewModel viewModel2 = ViewModelHelper.CreateLoginViewModel();
            viewModel2.UserName = viewModel.UserName;
            viewModel2.SourceAction = viewModel.SourceAction;
            return viewModel2;
        }
    }
}
