using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers;
using JJ.Framework.Common;
using JJ.Framework.Presentation;
using JJ.Models.Canonical;
using JJ.Apps.QuestionAndAnswer.Mvc.Views;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class QuestionsController : MasterController
    {
        public QuestionsController()
        {
            // The request must not be validated because so much text in the questions and answers will contain '<' and '>' characters,
            // because many questions are about HTML.
            ValidateRequest = false;
        }

        // GET: /Questions
        // GET: /Questions/Index

        public ActionResult Index()
        {
            QuestionListPresenter presenter = CreateListPresenter();
            QuestionListViewModel viewModel = presenter.Show();
            return View(viewModel);
        }

        // GET: /Questions/Details/5

        public ActionResult Details(int id)
        {
            QuestionDetailPresenter presenter = CreateDetailPresenter();
            object viewModel = presenter.Show(id);

            var detailViewModel = viewModel as QuestionDetailViewModel;
            if (detailViewModel != null)
            {
                return View(detailViewModel);
            }

            var notFoundViewModel = viewModel as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            throw new UnexpectedViewModelTypeException(viewModel);
        }

        // GET: /Questions/Edit/5

        public ActionResult Edit(int id)
        {
            object viewModel;
            if (TempData.ContainsKey(TempDataKeys.ViewModel))
            {
                viewModel = TempData[TempDataKeys.ViewModel];
            }
            else
            {
                QuestionDetailPresenter presenter = CreateDetailPresenter();
                viewModel = presenter.Show(id);
            }

            var detailViewModel = viewModel as QuestionDetailViewModel;
            if (detailViewModel != null)
            {
                foreach (ValidationMessage validationMessage in detailViewModel.ValidationMessages)
                {
                    ModelState.AddModelError(validationMessage.PropertyKey, validationMessage.Text);
                }

                return View(detailViewModel);
            }

            var notFoundViewModel = viewModel as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            throw new UnexpectedViewModelTypeException(viewModel);
        }

        // POST: /Questions/Edit/5

        /// <summary> Post edit. Saves the question. </summary>
        [HttpPost]
        public ActionResult Edit(QuestionDetailViewModel viewModel)
        {
            QuestionDetailPresenter presenter = CreateDetailPresenter();
            QuestionDetailViewModel viewModel2 = presenter.Save(viewModel);

            // TODO: Make this navigation logic part of the presenter.
            if (viewModel2.ValidationMessages.Count > 0)
            {
                TempData[TempDataKeys.ViewModel] = viewModel2;
                return RedirectToAction(ActionNames.Edit, new { id = viewModel2.Question.ID });
            }
            else
            {
                return RedirectToAction(ActionNames.Details, new { id = viewModel2.Question.ID });
            }
        }

        // POST: /Questions/AddLink

        [HttpPost]
        public ActionResult AddLink(QuestionDetailViewModel viewModel)
        {
            QuestionDetailPresenter presenter = CreateDetailPresenter();
            QuestionDetailViewModel viewModel2 = presenter.AddLink(viewModel);
            TempData[TempDataKeys.ViewModel] = viewModel2;
            return RedirectToAction(ActionNames.Edit, new { id = viewModel2.Question.ID });
        }

        // POST: /Questions/RemoveLink?temporaryID=12345678-90AB-CDEF

        [HttpPost]
        public ActionResult RemoveLink(QuestionDetailViewModel viewModel, Guid temporaryID)
        {
            QuestionDetailPresenter presenter = CreateDetailPresenter();
            QuestionDetailViewModel viewModel2 = presenter.RemoveLink(viewModel, temporaryID);
            TempData[TempDataKeys.ViewModel] = viewModel2;
            return RedirectToAction(ActionNames.Edit, new { id = viewModel2.Question.ID });
        }

        // POST: /Questions/AddCategory

        [HttpPost]
        public ActionResult AddCategory(QuestionDetailViewModel viewModel)
        {
            QuestionDetailPresenter presenter = CreateDetailPresenter();
            QuestionDetailViewModel viewModel2 = presenter.AddCategory(viewModel);
            TempData[TempDataKeys.ViewModel] = viewModel2;
            return RedirectToAction(ActionNames.Edit, new { id = viewModel2.Question.ID });
        }

        // POST: /Questions/RemoveCategory?temporaryID=12345678-90AB-CDEF

        [HttpPost]
        public ActionResult RemoveCategory(QuestionDetailViewModel viewModel, Guid temporaryID)
        {
            QuestionDetailPresenter presenter = CreateDetailPresenter();
            QuestionDetailViewModel viewModel2 = presenter.RemoveCategory(viewModel, temporaryID);
            TempData[TempDataKeys.ViewModel] = viewModel2;
            return RedirectToAction(ActionNames.Edit, new { id = viewModel2.Question.ID });
        }

        // GET: /Questions/Random
        // GET: /Questions/Random?c=1&c=2

        public ViewResult Random(int[] c)
        {
            RandomQuestionPresenter presenter = CreateRandomQuestionPresenter();
            
            object viewModel = presenter.Show(c);

            var randomQuestionViewModel = viewModel as RandomQuestionViewModel;
            if (randomQuestionViewModel != null)
            {
                return View(randomQuestionViewModel);
            }

            var notFoundViewModel = viewModel as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            throw new UnexpectedViewModelTypeException(viewModel);
        }

        // POST: /Questions/ShowAnswer/5

        [HttpPost]
        public ViewResult ShowAnswer(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            RandomQuestionPresenter presenter = CreateRandomQuestionPresenter();
            object viewModel2 = presenter.ShowAnswer(viewModel, authenticatedUserName);

            var randomQuestionViewModel = viewModel2 as RandomQuestionViewModel;
            if (randomQuestionViewModel != null)
            {
                return View(ViewNames.Random, randomQuestionViewModel);
            }

            var notFoundViewModel = viewModel2 as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            throw new UnexpectedViewModelTypeException(viewModel2);
        }

        // POST: /Questions/HideAnswer/5

        [HttpPost]
        public ViewResult HideAnswer(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            RandomQuestionPresenter presenter = CreateRandomQuestionPresenter();
            object viewModel2 = presenter.HideAnswer(viewModel, authenticatedUserName);

            var randomQuestionViewModel = viewModel2 as RandomQuestionViewModel;
            if (randomQuestionViewModel != null)
            {
                return View(ViewNames.Random, randomQuestionViewModel);
            }

            var notFoundViewModel = viewModel2 as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            throw new UnexpectedViewModelTypeException(viewModel2);
        }

        // POST: /Question/Flag/5

        [HttpPost]
        public ViewResult Flag(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            RandomQuestionPresenter presenter = CreateRandomQuestionPresenter();
            object viewModel2 = presenter.Flag(viewModel, authenticatedUserName);

            var randomQuestionViewModel = viewModel2 as RandomQuestionViewModel;
            if (randomQuestionViewModel != null)
            {
                return View(ViewNames.Random, randomQuestionViewModel);
            }

            var notFoundViewModel = viewModel2 as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            var notAuthenticatedViewModel = viewModel2 as NotAuthenticatedViewModel;
            if (notAuthenticatedViewModel != null)
            {
                return View(ViewNames.NotAuthenticated);
            }

            throw new UnexpectedViewModelTypeException(viewModel2);
        }

        // POST: /Question/Unflag/5

        [HttpPost]
        public ViewResult Unflag(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            RandomQuestionPresenter presenter = CreateRandomQuestionPresenter();
            object viewModel2 = presenter.Unflag(viewModel, authenticatedUserName);


            var randomQuestionViewModel = viewModel2 as RandomQuestionViewModel;
            if (randomQuestionViewModel != null)
            {
                return View(ViewNames.Random, randomQuestionViewModel);
            }

            var notFoundViewModel = viewModel2 as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            var notAuthenticatedViewModel = viewModel2 as NotAuthenticatedViewModel;
            if (notAuthenticatedViewModel != null)
            {
                return View(ViewNames.NotAuthenticated);
            }

            throw new UnexpectedViewModelTypeException(viewModel2);
        }

        // Helpers

        private RandomQuestionPresenter CreateRandomQuestionPresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            ICategoryRepository categoryRepository = RepositoryFactory.CreateCategoryRepository(context);
            IQuestionFlagRepository questionFlagRepository = RepositoryFactory.CreateQuestionFlagRepository(context);
            IFlagStatusRepository flagStatusRepository = RepositoryFactory.CreateFlagStatusRepository(context);
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            return new RandomQuestionPresenter(questionRepository, categoryRepository, questionFlagRepository, flagStatusRepository, userRepository);
        }

        private QuestionDetailPresenter CreateDetailPresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            IAnswerRepository answerRepository = RepositoryFactory.CreateAnswerRepository(context);
            ICategoryRepository categoryRepository = RepositoryFactory.CreateCategoryRepository(context);
            IQuestionCategoryRepository questionCategoryRepository = RepositoryFactory.CreateQuestionCategoryRepository(context);
            IQuestionLinkRepository questionLinkRepository = RepositoryFactory.CreateQuestionLinkRepository(context);
            IQuestionFlagRepository questionFlagRepository = RepositoryFactory.CreateQuestionFlagRepository(context);
            IFlagStatusRepository flagStatusRepository = RepositoryFactory.CreateFlagStatusRepository(context);
            return new QuestionDetailPresenter(questionRepository, answerRepository, categoryRepository, questionCategoryRepository, questionLinkRepository, questionFlagRepository, flagStatusRepository);
        }

        private QuestionListPresenter CreateListPresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            return new QuestionListPresenter(questionRepository);
        }
    }
}
