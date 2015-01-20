using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Helpers
{
    public class SessionWrapper
    {
        private HttpSessionStateBase _session;

        public SessionWrapper(HttpSessionStateBase session)
        {
            if (session == null) throw new NullException(() => session);
            _session = session;
        }

        public string AuthenticatedUserName
        {
            get { return (string)_session[SessionKeys.AuthenticatedUserName]; }
            set { _session[SessionKeys.AuthenticatedUserName] = value; }
        }

        public LoginPartialViewModel LoginPartialViewModel
        {
            get { return (LoginPartialViewModel)_session[SessionKeys.LoginPartialViewModel]; }
            set { _session[SessionKeys.LoginPartialViewModel] = value; }
        }
    }
}
