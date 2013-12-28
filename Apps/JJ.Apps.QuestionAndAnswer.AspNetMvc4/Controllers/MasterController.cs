using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    // TODO: Summary text is obsolete.
    /// <summary>
    /// Provides basic view data and basic actions, such as setting the language.
    /// Adds the HeaderViewModel for the master page to the ViewData dictionary with the key "HeaderViewModel".
    /// </summary>
    public abstract class MasterController : Controller
    {
        public abstract ActionResult Index();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // In the constructor I have no Session.
            InitializeSmallLoginSubController();

            OnActionExecuting_SetCurrentCulture();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            OnActionExecuted_SetLanguageSelectionViewModel();

            OnActionExecuted_EnsureSmallLoginViewModel();
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

            return RedirectToAction(ActionNames.Question, ControllerNames.Questions);
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

        private SmallLoginSubController _smallLoginSubController;

        private void InitializeSmallLoginSubController()
        {
            _smallLoginSubController = new SmallLoginSubController(Session);
        }

        public SmallLoginViewModel GetSmallLoginViewModel()
        {
            return _smallLoginSubController.GetSmallLoginViewModel();
        }

        protected string TryGetAuthenticatedUserName()
        {
            return GetSessionWrapper().AuthenticatedUserName;
        }

        // TODO: Replace ensure with just an auto-instantiating getter?
        private void OnActionExecuted_EnsureSmallLoginViewModel()
        {
            _smallLoginSubController.EnsureSmallLoginViewModel();
        }

        public ActionResult SetAuthenticatedUserName(string authenticatedUserName)
        {
            GetSessionWrapper().AuthenticatedUserName = authenticatedUserName;

            _smallLoginSubController.SetLoggedInUserName(authenticatedUserName);

            // What an assumption that we would want to go to the Question page. I would like to redirect to the page we were on before we tried to log in.
            return RedirectToAction(ActionNames.Question, ControllerNames.Questions);
        }

        public ActionResult LogOut()
        {
            GetSessionWrapper().AuthenticatedUserName = null;

            _smallLoginSubController.SetIsLoggedOut();

            // TODO: It feels strange that the presenter does not determine the program flow.
            return RedirectToAction(ActionNames.Index, ControllerNames.Login);
        }
    }
}


