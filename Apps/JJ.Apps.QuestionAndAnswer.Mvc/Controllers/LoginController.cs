using JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
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
            LoginPresenter presenter = CreatePresenter();
            LoginViewModel viewModel = presenter.Show();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel)
        {
            string password = viewModel.Password;
            string securityToken = viewModel.SecurityToken;

            LoginPresenter presenter = CreatePresenter();
            LoginViewModel viewModel2 = presenter.Login(password, securityToken, viewModel);

            if (!viewModel2.IsAuthenticated)
            {
                return View(viewModel2);
            }

            return base.SetAuthenticatedUserName(viewModel2.UserName);
        }

        private LoginPresenter CreatePresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            LoginPresenter presenter = new LoginPresenter(userRepository);
            return presenter;
        }
    }
}
