using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Framework.Common;
using JJ.Framework.Presentation;
using JJ.Business.CanonicalModel;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories;
using JJ.Framework.Configuration;
using JJ.Framework.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class QuestionsController : MasterController
    {
        public QuestionsController()
        {
            ValidateRequest = false;
        }

        public ActionResult Index(int page = 1)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionListPresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel = presenter.Show(page);
                }
            }

            return GetActionResult(ActionNames.Index, viewModel);
        }

        public ActionResult Details(int id)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionDetailsPresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel = presenter.Show(id);
                }
            }

            return GetActionResult(ActionNames.Details, viewModel);
        }

        public ActionResult Create()
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel = presenter.Create();
                }
            }

            return GetActionResult(ActionNames.Create, viewModel);
        }

        [HttpPost]
        public ActionResult Create(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.Save(viewModel);
                return GetActionResult(ActionNames.Create, viewModel2);
            }
        }

        public ActionResult Edit(int id)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel = presenter.Edit(id);
                }
            }

            return GetActionResult(ActionNames.Edit, viewModel);
        }

        [HttpPost]
        public ActionResult Edit(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.Save(viewModel);
                return GetActionResult(ActionNames.Edit, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult CancelEdit(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.Cancel(viewModel);
                return GetActionResult(ActionNames.Edit, viewModel2);
            }
        }

        public ActionResult Delete(int id)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionConfirmDeletePresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel = presenter.Show(id);
                }
            }

            return GetActionResult(ActionNames.Delete, viewModel);
        }

        [HttpPost]
        public ActionResult Delete(QuestionConfirmDeleteViewModel viewModel, int id)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionConfirmDeletePresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.Confirm(id);
                return GetActionResult(ActionNames.Delete, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult AddLink(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.AddLink(viewModel);
                return GetActionResult(ActionNames.AddLink, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult RemoveLink(QuestionEditViewModel viewModel, Guid temporaryID)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.RemoveLink(viewModel, temporaryID);
                return GetActionResult(ActionNames.RemoveLink, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult AddCategory(QuestionEditViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.AddCategory(viewModel);
                return GetActionResult(ActionNames.AddCategory, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult RemoveCategory(QuestionEditViewModel viewModel, Guid temporaryID)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2 = presenter.RemoveCategory(viewModel, temporaryID);
                return GetActionResult(ActionNames.RemoveCategory, viewModel2);
            }
        }

        public ActionResult Random(int[] c)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
                    viewModel = presenter.Show(c);
                }
            }

            return GetActionResult(ActionNames.Random, viewModel);
        }

        [HttpPost]
        public ActionResult Random(RandomQuestionViewModel viewModel, string lang)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.SetLanguage(viewModel, lang);
                CultureWebHelper.SetCultureCookie(ControllerContext.HttpContext, lang);
                return GetActionResult(ActionNames.Random, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult ShowAnswer(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.ShowAnswer(viewModel);
                return GetActionResult(ActionNames.ShowAnswer, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult HideAnswer(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.HideAnswer(viewModel);
                return GetActionResult(ActionNames.HideAnswer, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult Flag(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.Flag(viewModel);
                return GetActionResult(ActionNames.Flag, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult Unflag(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.Unflag(viewModel);
                return GetActionResult(ActionNames.Unflag, viewModel2);
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
                repositories.UserRepository,
                TryGetAuthenticatedUserName());
        }
    }
}
