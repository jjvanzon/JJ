using System.Web.Mvc;
using JJ.Framework.Data;
using JJ.Framework.Web;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using ActionDispatcher = JJ.Presentation.QuestionAndAnswer.Mvc.Helpers.ActionDispatcher;
// ReSharper disable UnusedParameter.Global
// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable RedundantIfElseBlock

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class LoginController : MasterController
    {
        public ActionResult Index(string ret)
        {
            if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new LoginPresenter(repositories);
                    viewModel = presenter.Show();
                }
            }

            return ActionDispatcher.Dispatch(this, viewModel);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel userInput, string lang = null, string ret = null)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new LoginPresenter(repositories);
                LoginViewModel viewModel;

                if (!string.IsNullOrEmpty(lang))
                {
                    viewModel = presenter.SetLanguage(userInput, lang);
                    CultureWebHelper.SetCultureCookie(ControllerContext.HttpContext, lang);
                }
                else
                {
                    viewModel = presenter.Login(userInput);
                }

                if (viewModel.IsAuthenticated)
                {
                    SetAuthenticatedUserName(userInput.UserName);
                }

                if (!string.IsNullOrEmpty(ret))
                {
                    return Redirect(ret);
                }
                else
                {
                    return Redirect("~/");
                }
            }
        }

        public ActionResult LogOut()
        {
            SessionWrapper.AuthenticatedUserName = null;

            return RedirectToAction(nameof(ActionNames.Index), nameof(ControllerNames.Login));
        }
    }
}