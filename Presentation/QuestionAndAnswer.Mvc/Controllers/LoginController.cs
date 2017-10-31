using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Framework.Data;
using System;
using System.Web.Mvc;
using JJ.Framework.Web;
using JJ.Framework.Presentation;
using ActionDispatcher = JJ.Framework.Presentation.Mvc.ActionDispatcher;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class LoginController : MasterController
    {
        public ActionResult Index(string ret = null)
        {
            object viewModel;
            if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    LoginPresenter presenter = new LoginPresenter(repositories);
                    ActionInfo returnAction = ActionDispatcher.TryGetActionInfo(ret);
                    viewModel = presenter.Show(returnAction);
                }
            }

            return ActionDispatcher.Dispatch(this, ActionNames.Index, viewModel);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel, string lang = null, string ret = null)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                LoginPresenter presenter = new LoginPresenter(repositories);
                object viewModel2;
                if (!string.IsNullOrEmpty(lang))
                {
                    viewModel2 = presenter.SetLanguage(viewModel, lang);
                    CultureWebHelper.SetCultureCookie(ControllerContext.HttpContext, lang);
                }
                else
                {
                    viewModel.ReturnAction = ActionDispatcher.TryGetActionInfo(ret);
                    viewModel2 = presenter.Login(viewModel);
                }

                // TODO: This is dirty.
                if (!(viewModel2 is LoginViewModel))
                {
                    SetAuthenticatedUserName(viewModel.UserName);
                }

                return ActionDispatcher.Dispatch(this, ActionNames.Index, viewModel2);
            }
        }

        public ActionResult LogOut()
        {
            SessionWrapper.AuthenticatedUserName = null;

            return RedirectToAction(ActionNames.Index, ControllerNames.Login);
        }
    }
}
