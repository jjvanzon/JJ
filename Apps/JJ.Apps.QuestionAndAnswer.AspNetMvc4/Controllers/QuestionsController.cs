using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Framework.Common;

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
            QuestionPresenter presenter = CreatePresenter();

            QuestionDetailViewModel viewModel = presenter.ShowQuestion(c);

            if (viewModel.NotFound)
            {
                return View(ViewNames.NotFound);
            }

            return View(viewModel);
        }

        // POST: /Questions/ShowAnswer/5

        [HttpPost]
        public ViewResult ShowAnswer(QuestionDetailViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            QuestionPresenter presenter = CreatePresenter();
            QuestionDetailViewModel viewModel2 = presenter.ShowAnswer(viewModel, authenticatedUserName);

            if (viewModel2.NotFound)
            {
                return View(ViewNames.NotFound);
            }

            return View(ViewNames.Question, viewModel2);
        }

        // POST: /Questions/HideAnswer/5

        [HttpPost]
        public ViewResult HideAnswer(QuestionDetailViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            QuestionPresenter presenter = CreatePresenter();
            QuestionDetailViewModel viewModel2 = presenter.HideAnswer(viewModel, authenticatedUserName);

            if (viewModel2.NotFound)
            {
                return View(ViewNames.NotFound);
            }

            return View(ViewNames.Question, viewModel2);
        }

        // POST: /Question/Flag/5

        [HttpPost]
        public ViewResult Flag(QuestionDetailViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            QuestionPresenter presenter = CreatePresenter();
            object viewModel2 = presenter.Flag(viewModel, authenticatedUserName);

            Type viewType = viewModel2.GetType();

            if (viewType == typeof(QuestionDetailViewModel))
            {
                return View(ViewNames.Question, viewModel2);
            }
            else if (viewType == typeof(NotAuthenticatedViewModel))
            {
                return View(ViewNames.NotAuthenticated);
            }
            else
            {
                throw new ValueNotSupportedException(viewType);
            }
        }

        // POST: /Question/Unflag/5

        [HttpPost]
        public ViewResult Unflag(QuestionDetailViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            QuestionPresenter presenter = CreatePresenter();
            object viewModel2 = presenter.Unflag(viewModel, authenticatedUserName);

            Type viewType = viewModel2.GetType();

            if (viewType == typeof(QuestionDetailViewModel))
            {
                return View(ViewNames.Question, viewModel2);
            }
            else if (viewType == typeof(NotAuthenticatedViewModel))
            {
                return View(ViewNames.NotAuthenticated);
            }

            throw new ValueNotSupportedException(viewType);
        }

        // Private Methods

        private QuestionPresenter CreatePresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            ICategoryRepository categoryRepository = RepositoryFactory.CreateCategoryRepository(context);
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            IQuestionFlagRepository questionFlagRepository = RepositoryFactory.CreateQuestionFlagRepository(context);
            IFlagStatusRepository flagStatusRepository = RepositoryFactory.CreateFlagStatusRepository(context);
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            return new QuestionPresenter(questionRepository, categoryRepository, questionFlagRepository, flagStatusRepository, userRepository);
        }
    }
}
