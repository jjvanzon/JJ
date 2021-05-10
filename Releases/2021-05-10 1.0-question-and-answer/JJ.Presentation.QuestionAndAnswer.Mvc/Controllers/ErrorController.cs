using System.Web.Mvc;
using JJ.Framework.Web;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class ErrorController : MasterController
    {
        public ActionResult Index()
        {
            Response.StatusCode = HttpStatusCodes.INTERNAL_SERVER_ERROR_500;
            return View(nameof(ViewNames.Error));
        }
    }
}