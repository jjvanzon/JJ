using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.ViewModels;
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

        public string CultureName
        {
            get { return (string)_session[SessionKeys.CultureName]; }
            set { _session[SessionKeys.CultureName] = value; }
        }

        public string AuthenticatedUserName
        {
            get { return (string)_session[SessionKeys.AuthenticatedUserName]; }
            set { _session[SessionKeys.AuthenticatedUserName] = value; }
        }

        public SmallLoginViewModel SmallLoginViewModel
        {
            get { return (SmallLoginViewModel)_session[SessionKeys.SmallLoginViewModel]; }
            set { _session[SessionKeys.SmallLoginViewModel] = value; }
        }
    }
}
