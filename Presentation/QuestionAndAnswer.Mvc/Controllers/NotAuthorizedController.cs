using System.Web.Mvc;
using JJ.Framework.Web;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class NotAuthorizedController : MasterController
    {
        public ActionResult Index()
        {
            Response.StatusCode = HttpStatusCodes.NOT_AUTHORIZED_403;
            return View(nameof(ViewNames.NotAuthorized));
        }
    }
}