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
    /// <summary>
    /// Provides basic view data and basic actions, such as setting the language and a small login widget.
    /// </summary>
    public abstract class MasterController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // In the constructor there is no Session.

            InitializeSmallLoginSubController();

            SetCurrentCulture();
        }

        private SessionWrapper GetSessionWrapper()
        {
            return new SessionWrapper(Session);
        }

        // Language

        private void SetCurrentCulture()
        {
            string cultureName = GetSessionWrapper().CultureName;

            if (!String.IsNullOrEmpty(cultureName))
            {
                CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            }
        }

        public ActionResult SetLanguage(string cultureName)
        {
            var presenter = new LanguageSelectionPresenter();

            LanguageSelectionViewModel viewModel = presenter.SetLanguage(cultureName);

            LanguageSelectionViewModel = viewModel;

            GetSessionWrapper().CultureName = viewModel.SelectedLanguageCultureName;

            return RedirectToAction(ActionNames.Random, ControllerNames.Questions);
        }

        public LanguageSelectionViewModel LanguageSelectionViewModel
        {
            get
            {
                LanguageSelectionViewModel viewModel = (LanguageSelectionViewModel)ViewData[ViewDataKeys.LanguageSelectionViewModel];

                if (viewModel == null)
                {
                    var presenter = new LanguageSelectionPresenter();
                    viewModel = presenter.Show();
                    ViewData[ViewDataKeys.LanguageSelectionViewModel] = viewModel;
                }

                return viewModel;
            }
            private set
            {
                ViewData[ViewDataKeys.LanguageSelectionViewModel] = value;
            }
        }

        // Login

        private SmallLoginSubController _smallLoginSubController;

        private void InitializeSmallLoginSubController()
        {
            _smallLoginSubController = new SmallLoginSubController(this);
        }

        public SmallLoginViewModel SmallLoginViewModel
        {
            get { return _smallLoginSubController.Model; }
        }

        protected string TryGetAuthenticatedUserName()
        {
            return GetSessionWrapper().AuthenticatedUserName;
        }

        public ActionResult SetAuthenticatedUserName(string authenticatedUserName)
        {
            GetSessionWrapper().AuthenticatedUserName = authenticatedUserName;

            _smallLoginSubController.SetLoggedInUserName(authenticatedUserName);

            // What an assumption that we would want to go to the Question page. I would like to redirect to the page we were on before we tried to log in.
            return RedirectToAction(ActionNames.Random, ControllerNames.Questions);
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


