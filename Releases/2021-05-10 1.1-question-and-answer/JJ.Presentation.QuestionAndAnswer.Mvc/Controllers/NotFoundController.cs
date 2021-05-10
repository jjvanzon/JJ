using System.Web.Mvc;
using JJ.Framework.Web;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class NotFoundController : MasterController
    {
        public ActionResult Index()
        {
            Response.StatusCode = HttpStatusCodes.NOT_FOUND_404;
            return View(nameof(ViewNames.NotFound));
        }
    }
}