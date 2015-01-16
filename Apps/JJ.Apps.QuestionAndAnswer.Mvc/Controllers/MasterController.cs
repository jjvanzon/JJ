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
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.Presenters.Partials;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;

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
            LanguageSelectorPartialPresenter presenter = new LanguageSelectorPartialPresenter();

            LanguageSelectorPartialViewModel viewModel = presenter.SetLanguage(cultureName);

            LanguageSelectorPartialViewModel = viewModel;

            GetSessionWrapper().CultureName = viewModel.SelectedLanguageCultureName;

            return RedirectToAction(ActionNames.Random, ControllerNames.Questions);
        }

        public LanguageSelectorPartialViewModel LanguageSelectorPartialViewModel
        {
            get
            {
                LanguageSelectorPartialViewModel viewModel = (LanguageSelectorPartialViewModel)ViewData[ViewDataKeys.LanguageSelectorPartialViewModel];

                if (viewModel == null)
                {
                    var presenter = new LanguageSelectorPartialPresenter();
                    viewModel = presenter.Show();
                    ViewData[ViewDataKeys.LanguageSelectorPartialViewModel] = viewModel;
                }

                return viewModel;
            }
            private set
            {
                ViewData[ViewDataKeys.LanguageSelectorPartialViewModel] = value;
            }
        }

        // Login

        protected string TryGetAuthenticatedUserName()
        {
            return GetSessionWrapper().AuthenticatedUserName;
        }

        public void SetAuthenticatedUserName(string authenticatedUserName)
        {
            GetSessionWrapper().AuthenticatedUserName = authenticatedUserName;
        }

        public ActionResult LogOut()
        {
            GetSessionWrapper().AuthenticatedUserName = null;

            return RedirectToAction(ActionNames.Index, ControllerNames.Login);
        }
    }
}
