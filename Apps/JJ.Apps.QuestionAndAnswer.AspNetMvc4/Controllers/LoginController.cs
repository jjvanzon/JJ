using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class LoginController : MasterController
    {
        public override ActionResult Index()
        {
            LoginPresenter presenter = CreatePresenter();
            LoginViewModel viewModel = presenter.Show();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel)
        {
            LoginPresenter presenter = CreatePresenter();
            LoginViewModel viewModel2 = presenter.Login(viewModel);

            // TODO: Application navigation should become the presenters' reposibility.
            if (!viewModel2.IsLoggedIn)
            {
                return View(viewModel2);
            }
            else
            {
                base.SetLoginViewModel(viewModel2);

                // What an assumption that we would want to go to the Question page. I would like to redirect to the page we were on before we tried to log in.
                return RedirectToAction(ActionNames.Question, ControllerNames.Questions);
            }
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
