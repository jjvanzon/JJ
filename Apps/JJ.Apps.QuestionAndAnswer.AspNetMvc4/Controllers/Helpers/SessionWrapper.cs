using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers
{
    public class SessionWrapper
    {
        private HttpSessionStateBase _session;

        public SessionWrapper(HttpSessionStateBase session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            
            _session = session;
        }

        public string CultureName
        {
            get { return (string)_session[SessionKeys.CultureName]; }
            set { _session[SessionKeys.CultureName] = value; }
        }

        public LoginViewModel LoginViewModel
        {
            get { return (LoginViewModel)_session[SessionKeys.LoginViewModel]; }
            set { _session[SessionKeys.LoginViewModel] = value; }
        }
    }
}