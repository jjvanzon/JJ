using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Repositories;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
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
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                LoginPresenter presenter = new LoginPresenter(userRepository);
                LoginViewModel viewModel = presenter.Show();
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                LoginPresenter presenter = new LoginPresenter(userRepository);

                string password = viewModel.Password;
                string securityToken = viewModel.SecurityToken;

                LoginViewModel viewModel2 = presenter.Login(password, securityToken, viewModel);

                if (!viewModel2.IsAuthenticated)
                {
                    return View(viewModel2);
                }

                return base.SetAuthenticatedUserName(viewModel2.UserName);
            }
        }
    }
}
