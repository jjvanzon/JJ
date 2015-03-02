using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Framework.Presentation.Mvc;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class LoginController : MasterController
    {
        public ActionResult Index()
        {
            object viewModel;
            if (!TempData.TryGetValue(ActionDispatcher.VIEW_MODEL_TEMP_DATA_KEY, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    LoginPresenter presenter = new LoginPresenter(repositories);
                    viewModel = presenter.Show();
                }
            }

            return ActionDispatcher.DispatchAction(this, ActionNames.Index, viewModel);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel, string lang = null)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                LoginPresenter presenter = new LoginPresenter(repositories);
                object viewModel2;
                if (!String.IsNullOrEmpty(lang))
                {
                    viewModel2 = presenter.SetLanguage(viewModel, lang);
                    CultureWebHelper.SetCultureCookie(ControllerContext.HttpContext, lang);
                }
                else
                {
                    viewModel2 = presenter.Login(viewModel);
                }

                // TODO: This is dirty.
                if (!(viewModel2 is LoginViewModel))
                {
                    SetAuthenticatedUserName(viewModel.UserName);
                }

                return ActionDispatcher.DispatchAction(this, ActionNames.Index, viewModel2);
            }
        }

        public ActionResult LogOut()
        {
            SessionWrapper.AuthenticatedUserName = null;

            return RedirectToAction(ActionNames.Index, ControllerNames.Login);
        }
    }
}
