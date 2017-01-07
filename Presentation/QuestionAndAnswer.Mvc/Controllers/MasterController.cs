using System.Web.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
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
