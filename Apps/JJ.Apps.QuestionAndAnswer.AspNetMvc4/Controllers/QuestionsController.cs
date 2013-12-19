using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class QuestionsController : MasterController
    {
        public QuestionsController()
        {
            ValidateRequest = false;
        }

        public override ActionResult Index()
        {
            return Question(null);
        }

        // GET: /Questions/Question
        // GET: /Questions/Question?categoryID=1&categoryID=2

        public ViewResult Question(int[] c)
        {
            using (QuestionPresenter presenter = new QuestionPresenter())
            {
                QuestionDetailViewModel viewModel = presenter.ShowQuestion(c);

                if (viewModel.NotFound)
                {
                    return View(ViewNames.NotFound);
                }

                return View(ViewNames.Question, viewModel);
            }
        }

        // POST: /Questions/ShowAnswer/5

        [HttpPost]
        public ViewResult ShowAnswer(QuestionDetailViewModel viewModel)
        {
            using (QuestionPresenter presenter = new QuestionPresenter())
            {
                QuestionDetailViewModel viewModel2 = presenter.ShowAnswer(viewModel);

                if (viewModel2.NotFound)
                {
                    return View(ViewNames.NotFound);
                }

                return View(ViewNames.Question, viewModel2);
            }
        }

        // POST: /Questions/HideAnswer/5

        [HttpPost]
        public ViewResult HideAnswer(QuestionDetailViewModel viewModel)
        {
            using (QuestionPresenter presenter = new QuestionPresenter())
            {
                QuestionDetailViewModel viewModel2 = presenter.HideAnswer(viewModel);

                if (viewModel2.NotFound)
                {
                    return View(ViewNames.NotFound);
                }

                return View(ViewNames.Question, viewModel2);
            }
        }

        // POST: /Question/Flag/5

        [HttpPost]
        public ViewResult Flag(QuestionDetailViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
