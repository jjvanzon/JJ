using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using JJ.Framework.Common;
using JJ.Framework.Presentation;
using JJ.Models.Canonical;
using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class QuestionsController : MasterController
    {
        public QuestionsController()
        {
            ValidateRequest = false;
        }

        // GET: /Questions
        // GET: /Questions/Index

        public ActionResult Index()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionListPresenter presenter = new QuestionListPresenter(repositories);
                QuestionListViewModel viewModel = presenter.Show();
                return View(viewModel);
            }
        }

        // GET: /Questions/Details/5

        public ActionResult Details(int id)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionDetailsPresenter presenter = new QuestionDetailsPresenter(repositories);
                object viewModel = presenter.Show(id);

                var detailModel = viewModel as QuestionDetailsViewModel;
                if (detailModel != null)
                {
                    return View(detailModel);
                }

                var notFoundViewModel = viewModel as QuestionNotFoundViewModel;
                if (notFoundViewModel != null)
                {
                    return View(ViewNames.NotFound, notFoundViewModel);
                }

                throw new UnexpectedViewModelTypeException(viewModel);
            }
        }

        // GET: /Questions/Create

        public ActionResult Create()
        {
            object viewModel;
            if (TempData.ContainsKey(TempDataKeys.ViewModel))
            {
                viewModel = TempData[TempDataKeys.ViewModel];
            }
            else
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = CreateRepositories(context);
                    QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                    viewModel = presenter.Create();
                }
            }

            var editViewModel = viewModel as QuestionEditViewModel;
            if (editViewModel != null)
            {
                foreach (ValidationMessage validationMessage in editViewModel.ValidationMessages)
                {
                    ModelState.AddModelError(validationMessage.PropertyKey, validationMessage.Text);
                }

                return View(ViewNames.Edit, editViewModel);
            }

            var notFoundViewModel = viewModel as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                return View(ViewNames.NotFound, notFoundViewModel);
            }

            throw new UnexpectedViewModelTypeException(viewModel);
        }

        // POST: /Questions/Create

        /// <summary> Post create. Saves the new question. </summary>
        [HttpPost]
        public ActionResult Create(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                string authenticatedUserName = TryGetAuthenticatedUserName();
                object viewModel2 = presenter.Save(viewModel, authenticatedUserName);

                var editViewModel = viewModel2 as QuestionEditViewModel;
                if (editViewModel != null)
                {
                    TempData[TempDataKeys.ViewModel] = editViewModel;
                    return RedirectToAction(ActionNames.Create);
                }

                var detailsViewModel = viewModel2 as QuestionDetailsViewModel;
                if (detailsViewModel != null)
                {
                    return RedirectToAction(ActionNames.Details, new { id = detailsViewModel.Question.ID });
                }

                throw new UnexpectedViewModelTypeException(viewModel);
            }
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
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = CreateRepositories(context);
                    QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                    viewModel = presenter.Edit(id);
                }
            }

            var editViewModel = viewModel as QuestionEditViewModel;
            if (editViewModel != null)
            {
                foreach (ValidationMessage validationMessage in editViewModel.ValidationMessages)
                {
                    ModelState.AddModelError(validationMessage.PropertyKey, validationMessage.Text);
                }

                return View(editViewModel);
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
        public ActionResult Edit(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                string authenticatedUserName = TryGetAuthenticatedUserName();
                object viewModel2 = presenter.Save(viewModel, authenticatedUserName);

                var editViewModel = viewModel2 as QuestionEditViewModel;
                if (editViewModel != null)
                {
                    TempData[TempDataKeys.ViewModel] = editViewModel;
                    return RedirectToAction(ActionNames.Edit, new { id = editViewModel.Question.ID });
                }

                var detailsViewModel = viewModel2 as QuestionDetailsViewModel;
                if (detailsViewModel != null)
                {
                    return RedirectToAction(ActionNames.Details, new { id = detailsViewModel.Question.ID });
                }

                throw new UnexpectedViewModelTypeException(viewModel);
            }
        }

        // GET: /Questions/Delete/5

        /// <summary> Asks for confirmation that the question can be deleted. </summary>
        public ActionResult Delete(int id)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionConfirmDeletePresenter presenter = new QuestionConfirmDeletePresenter(repositories);
                object viewModel = presenter.Show(id);

                var confirmDeleteViewModel = viewModel as QuestionConfirmDeleteViewModel;
                if (confirmDeleteViewModel != null)
                {
                    return View(ViewNames.Delete, confirmDeleteViewModel);
                }

                var notFoundViewModel = viewModel as QuestionNotFoundViewModel;
                if (notFoundViewModel != null)
                {
                    return View(ViewNames.NotFound, notFoundViewModel);
                }

                throw new UnexpectedViewModelTypeException(viewModel);
            }
        }

        // POST: /Questions/Delete/5

        /// <summary>
        /// With this action the user confirms that the question can be deleted.
        /// </summary>
        /// <param name="viewModel">
        /// Do nothing with this view model. 
        /// It is only provided to give the HTTP get and post actions different method signatures.
        /// </param>
        [HttpPost]
        public ActionResult Delete(QuestionConfirmDeleteViewModel viewModel, int id)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionDeleteConfirmedPresenter presenter = new QuestionDeleteConfirmedPresenter(repositories);
                object viewModel2 = presenter.Show(id);

                var deleteConfirmedViewModel = viewModel2 as QuestionDeleteConfirmedViewModel;
                if (deleteConfirmedViewModel != null)
                {
                    return View(ViewNames.Deleted, deleteConfirmedViewModel);
                }

                var notFoundViewModel = viewModel2 as QuestionDeleteConfirmedViewModel;
                if (notFoundViewModel != null)
                {
                    return View(ViewNames.NotFound, notFoundViewModel);
                }

                throw new UnexpectedViewModelTypeException(viewModel2);
            }
        }

        // POST: /Questions/AddLink

        [HttpPost]
        public ActionResult AddLink(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                QuestionEditViewModel viewModel2 = presenter.AddLink(viewModel);
                TempData[TempDataKeys.ViewModel] = viewModel2;
                if (viewModel.IsNew)
                {
                    return RedirectToAction(ActionNames.Create);
                }
                else
                {
                    return RedirectToAction(ActionNames.Edit, new { id = viewModel.Question.ID });
                }
            }
        }

        // POST: /Questions/RemoveLink?temporaryID=12345678-90AB-CDEF

        [HttpPost]
        public ActionResult RemoveLink(QuestionEditViewModel viewModel, Guid temporaryID)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                QuestionEditViewModel viewModel2 = presenter.RemoveLink(viewModel, temporaryID);
                TempData[TempDataKeys.ViewModel] = viewModel2;
                if (viewModel.IsNew)
                {
                    return RedirectToAction(ActionNames.Create);
                }
                else
                {
                    return RedirectToAction(ActionNames.Edit, new { id = viewModel.Question.ID });
                }
            }
        }

        // POST: /Questions/AddCategory

        [HttpPost]
        public ActionResult AddCategory(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                QuestionEditViewModel viewModel2 = presenter.AddCategory(viewModel);
                TempData[TempDataKeys.ViewModel] = viewModel2;
                if (viewModel.IsNew)
                {
                    return RedirectToAction(ActionNames.Create);
                }
                else
                {
                    return RedirectToAction(ActionNames.Edit, new { id = viewModel.Question.ID });
                }
            }
        }

        // POST: /Questions/RemoveCategory?temporaryID=12345678-90AB-CDEF

        [HttpPost]
        public ActionResult RemoveCategory(QuestionEditViewModel viewModel, Guid temporaryID)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                QuestionEditPresenter presenter = new QuestionEditPresenter(repositories);
                QuestionEditViewModel viewModel2 = presenter.RemoveCategory(viewModel, temporaryID);
                TempData[TempDataKeys.ViewModel] = viewModel2;
                if (viewModel.IsNew)
                {
                    return RedirectToAction(ActionNames.Create);
                }
                else
                {
                    return RedirectToAction(ActionNames.Edit, new { id = viewModel.Question.ID });
                }
            }
        }

        // GET: /Questions/Random
        // GET: /Questions/Random?c=1&c=2

        public ViewResult Random(int[] c)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);

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
        }

        // POST: /Questions/ShowAnswer/5

        [HttpPost]
        public ViewResult ShowAnswer(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
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
        }

        // POST: /Questions/HideAnswer/5

        [HttpPost]
        public ViewResult HideAnswer(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
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
        }

        // POST: /Question/Flag/5

        [HttpPost]
        public ViewResult Flag(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
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
        }

        // POST: /Question/Unflag/5

        [HttpPost]
        public ViewResult Unflag(RandomQuestionViewModel viewModel)
        {
            string authenticatedUserName = TryGetAuthenticatedUserName();

            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
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
        }

        // Helpers

        private RandomQuestionPresenter CreateRandomQuestionPresenter(Repositories repositories)
        {
            return new RandomQuestionPresenter(
                repositories.QuestionRepository, 
                repositories.CategoryRepository, 
                repositories.QuestionFlagRepository, 
                repositories.FlagStatusRepository, 
                repositories.UserRepository);
        }

        private Repositories CreateRepositories(IContext context)
        {
            return new Repositories(
                PersistenceHelper.CreateRepository<IQuestionRepository>(context),
                PersistenceHelper.CreateRepository<IAnswerRepository>(context),
                PersistenceHelper.CreateRepository<ICategoryRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionCategoryRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionLinkRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionFlagRepository>(context),
                PersistenceHelper.CreateRepository<IFlagStatusRepository>(context),
                PersistenceHelper.CreateRepository<ISourceRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionTypeRepository>(context),
                PersistenceHelper.CreateRepository<IUserRepository>(context));
        }
    }
}
