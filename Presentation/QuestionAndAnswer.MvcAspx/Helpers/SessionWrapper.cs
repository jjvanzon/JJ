using JJ.Presentation.QuestionAndAnswer.MvcAspx.Names;
using JJ.Framework.Exceptions;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.MvcAspx.Helpers
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
	}
}
