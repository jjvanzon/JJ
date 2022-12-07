using System.Web;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Helpers
{
    public class SessionWrapper
    {
        private readonly HttpSessionStateBase _session;

        public SessionWrapper(HttpSessionStateBase session) => _session = session ?? throw new NullException(() => session);

        public string AuthenticatedUserName
        {
            get => (string)_session[nameof(SessionKeys.AuthenticatedUserName)];
            set => _session[nameof(SessionKeys.AuthenticatedUserName)] = value;
        }
    }
}