using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class QuestionsController : Controller
    {
        public QuestionsController()
        {
            ValidateRequest = false;
        }

        // GET: /Questions/

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Questions/Question
        // GET: /Questions/Question/5
        // ShowQuestion / NextQuestion

        public ActionResult Question(int? id = null)
        {
            using (QuestionPresenter presenter = new QuestionPresenter())
            {
                QuestionDetailViewModel viewModel;
                if (id.HasValue)
                {
                    viewModel = presenter.ShowQuestion(id.Value);
                }
                else
                {
                    viewModel = presenter.NextQuestion();
                }

                return View(viewModel);
            }
        }

        // POST: /Questions/Question/5
        // ShowAnswer

        [HttpPost]
        public ActionResult Question(QuestionDetailViewModel viewModel)
        {
            using (QuestionPresenter presenter = new QuestionPresenter())
            {
                QuestionDetailViewModel viewModel2 = presenter.ShowAnswer(viewModel);
                return View(viewModel2);
            }
        }
    }
}
