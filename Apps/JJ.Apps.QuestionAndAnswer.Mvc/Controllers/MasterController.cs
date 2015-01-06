using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    /// <summary>
    /// Provides basic view data and basic actions, such as setting the language and a small login widget.
    /// </summary>
    public abstract class MasterController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // In the constructor there is no Session. That is why we need to use OnActionExecuting.

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
            LanguageSelectorPresenter presenter = new LanguageSelectorPresenter();

            LanguageSelectorViewModel viewModel = presenter.SetLanguage(cultureName);

            LanguageSelectorViewModel = viewModel;

            GetSessionWrapper().CultureName = viewModel.SelectedLanguageCultureName;

            return RedirectToAction(ActionNames.Random, ControllerNames.Questions);
        }

        public LanguageSelectorViewModel LanguageSelectorViewModel
        {
            get
            {
                LanguageSelectorViewModel viewModel = (LanguageSelectorViewModel)ViewData[ViewDataKeys.LanguageSelectorViewModel];

                if (viewModel == null)
                {
                    var presenter = new LanguageSelectorPresenter();
                    viewModel = presenter.Show();
                    ViewData[ViewDataKeys.LanguageSelectorViewModel] = viewModel;
                }

                return viewModel;
            }
            private set
            {
                ViewData[ViewDataKeys.LanguageSelectorViewModel] = value;
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


