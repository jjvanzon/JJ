using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Data;
using JJ.Framework.Presentation;
using JJ.Framework.Presentation.Mvc;
using JJ.Data.Canonical;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using System.Threading;
using System.Collections.Specialized;
using JJ.Framework.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public abstract class MasterController : Controller
    {
        private const string DEFAULT_CULTURE_NAME = "en-US";

        protected SessionWrapper SessionWrapper { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionWrapper = new SessionWrapper(Session);

            CultureWebHelper.SetThreadCultureByHttpHeaderOrCookie(ControllerContext.HttpContext, DEFAULT_CULTURE_NAME);

            base.OnActionExecuting(filterContext);
        }

        // Login

        protected string TryGetAuthenticatedUserName()
        {
            return SessionWrapper.AuthenticatedUserName;
        }

        public void SetAuthenticatedUserName(string authenticatedUserName)
        {
            SessionWrapper.AuthenticatedUserName = authenticatedUserName;
        }
    }
}
