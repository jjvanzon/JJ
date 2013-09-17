using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views.Helpers;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class QuestionsController : Controller
    {
        // GET: /Question/

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Questions/Question
        // GET: /Questions/Question/5

        public ActionResult Question(int? id = null)
        {
            using (QuestionPresenter presenter = new QuestionPresenter(id))
            {
                QuestionDetailViewModel viewModel = presenter.ShowQuestion();
                return View(viewModel);
            }
        }

        // POST: /Questions/Question/5

        [HttpPost]
        public ActionResult Question(QuestionDetailViewModel viewModel)
        {
            using (QuestionPresenter presenter = new QuestionPresenter(viewModel.ID))
            {
                QuestionDetailViewModel viewModel2 = presenter.ShowAnswer(viewModel);
                return View(viewModel2);
            }
        }
    }
}
