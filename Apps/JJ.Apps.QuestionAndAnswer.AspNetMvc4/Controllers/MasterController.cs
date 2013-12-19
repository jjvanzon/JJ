using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Models.QuestionAndAnswer;
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
            OnActionExecuted_SetLanguageSelectionViewModel();
            OnActionExecuted_EnsureLoginViewModel();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            OnActionExecuting_SetCurrentCulture();
        }

        private SessionWrapper GetSessionWrapper()
        {
            return new SessionWrapper(Session);
        }

        // Language

        private void OnActionExecuted_SetLanguageSelectionViewModel()
        {
            var presenter = new LanguageSelectionPresenter();

            LanguageSelectionViewModel viewModel = presenter.Show();

            SetLanguageSelectionViewModel(viewModel);
        }

        private void OnActionExecuting_SetCurrentCulture()
        {
            string cultureName = GetSessionWrapper().CultureName;

            if (!String.IsNullOrEmpty(cultureName))
            {
                SetCurrentCulture(cultureName);
            }
        }

        public ActionResult SetLanguage(string cultureName)
        {
            var presenter = new LanguageSelectionPresenter();

            LanguageSelectionViewModel viewModel = presenter.SetLanguage(cultureName);

            SetLanguageSelectionViewModel(viewModel);

            GetSessionWrapper().CultureName = viewModel.SelectedLanguageCultureName;

            return RedirectToAction(ActionNames.Index);
        }

        public LanguageSelectionViewModel GetLanguageSelectionViewModel()
        {
            return (LanguageSelectionViewModel)ViewData[ViewDataKeys.LanguageSelectionViewModel];
        }

        private void SetLanguageSelectionViewModel(LanguageSelectionViewModel viewModel)
        {
            ViewData[ViewDataKeys.LanguageSelectionViewModel] = viewModel;
        }

        private void SetCurrentCulture(string cultureName)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        // Login

        private void OnActionExecuted_EnsureLoginViewModel()
        {
            LoginViewModel viewModel = GetLoginViewModel();

            if (viewModel == null)
            {
                InitializeLoginViewModel();
            }
        }

        public LoginViewModel GetLoginViewModel()
        {
            return GetSessionWrapper().LoginViewModel;
        }

        protected void SetLoginViewModel(LoginViewModel viewModel)
        {
            GetSessionWrapper().LoginViewModel = viewModel;
        }

        public ActionResult LogOut()
        {
            InitializeLoginViewModel();
            return RedirectToAction(ActionNames.Index, ControllerNames.Login);
        }

        private void InitializeLoginViewModel()
        {
            var presenter = new SmallLoginPresenter();
            LoginViewModel viewModel = presenter.Show();
            SetLoginViewModel(viewModel);
        }
    }
}


