using JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class LoginController : MasterController
    {
        public ActionResult Index()
        {
            using (UserRepositoryWrapper repositoryWrapper = CreateRepositoryWrapper())
            {
                LoginPresenter presenter = new LoginPresenter(repositoryWrapper.UserRepository);
                LoginViewModel viewModel = presenter.Show();
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel)
        {
            using (UserRepositoryWrapper repositoryWrapper = CreateRepositoryWrapper())
            {
                string password = viewModel.Password;
                string securityToken = viewModel.SecurityToken;

                LoginPresenter presenter = new LoginPresenter(repositoryWrapper.UserRepository);
                LoginViewModel viewModel2 = presenter.Login(password, securityToken, viewModel);

                if (!viewModel2.IsAuthenticated)
                {
                    return View(viewModel2);
                }

                return base.SetAuthenticatedUserName(viewModel2.UserName);
            }
        }

        private UserRepositoryWrapper CreateRepositoryWrapper()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            IUserRepository userRepository = new UserRepository(context);
            return new UserRepositoryWrapper(userRepository, context);
        }
    }
}
