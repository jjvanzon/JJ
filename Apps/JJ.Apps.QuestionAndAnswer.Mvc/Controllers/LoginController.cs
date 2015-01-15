using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Framework.Presentation;
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
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    LoginPresenter presenter = new LoginPresenter(repositories);
                    viewModel = presenter.Show();
                }
            }

            return GetActionResult(ActionNames.Index, viewModel);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                LoginPresenter presenter = new LoginPresenter(repositories);
                object viewModel2 = presenter.Login(viewModel);

                // TODO: This is dirty.
                if (!(viewModel2 is LoginViewModel))
                {
                    base.SetAuthenticatedUserName(viewModel.UserName);
                }

                return GetActionResult(ActionNames.Index, viewModel2);
            }
        }
    }
}
