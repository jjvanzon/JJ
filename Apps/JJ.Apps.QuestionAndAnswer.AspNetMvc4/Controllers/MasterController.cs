using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    /// <summary>
    /// Provides basic view data and basic actions, such as setting the language.
    /// Adds the HeaderViewModel for the master page to the ViewData dictionary with the key "HeaderViewModel".
    /// </summary>
    public abstract class MasterController : Controller
    {
        public abstract ActionResult Index();

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var presenter = new HeaderPresenter();

            HeaderViewModel viewModel = presenter.Show();

            SetHeaderViewModel(viewModel);
        }

        public ActionResult SetLanguage(string cultureName)
        {
            var presenter = new HeaderPresenter();

            HeaderViewModel viewModel = presenter.SetLanguage(cultureName);

            SetHeaderViewModel(viewModel);

            GetSessionWrapper().CultureName = viewModel.Language.SelectedLanguageCultureName;

            return RedirectToAction(ActionNames.Index);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cultureName = GetSessionWrapper().CultureName;

            if (!String.IsNullOrEmpty(cultureName))
            {
                SetCurrentCulture(cultureName);
            }
        }

        private void SetHeaderViewModel(HeaderViewModel viewModel)
        {
            ViewData[ViewDataKeys.HeaderViewModel] = viewModel;
        }

        private void SetCurrentCulture(string cultureName)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        private SessionWrapper GetSessionWrapper()
        {
            return new SessionWrapper(Session);
        }
    }
}


