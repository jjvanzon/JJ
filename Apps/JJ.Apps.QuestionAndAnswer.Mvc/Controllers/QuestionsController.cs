using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using JJ.Framework.Common;
using JJ.Framework.Presentation;
using JJ.Models.Canonical;
using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Models.QuestionAndAnswer.Repositories;

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

        public ActionResult Index(string lang = null)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionListPresenter(repositories, TryGetAuthenticatedUserName());

                    if (!String.IsNullOrEmpty(lang))
                    {
                        viewModel = presenter.SetLanguage(lang);
                        GetSessionWrapper().CultureName = lang;
                    }
                    else
                    {
                        viewModel = presenter.Show();
                    }
                }
            }

            return GetActionResult(ActionNames.Index, viewModel);
        }

        // GET: /Questions/Details/5

        public ActionResult Details(int id, string lang = null)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionDetailsPresenter(repositories, TryGetAuthenticatedUserName());

                    if (!String.IsNullOrEmpty(lang))
                    {
                        viewModel = presenter.SetLanguage(id, lang);
                        GetSessionWrapper().CultureName = lang;
                    }
                    else
                    {
                        viewModel = presenter.Show(id);
                    }
                }
            }

            return GetActionResult(ActionNames.Details, viewModel);
        }

        // GET: /Questions/Create

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

        // POST: /Questions/Create

        [HttpPost]
        public ActionResult Create(QuestionEditViewModel viewModel, string lang = null)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
                object viewModel2;
                if (!String.IsNullOrEmpty(lang))
                {
                    viewModel2 = presenter.SetLanguage(viewModel, lang);
                    GetSessionWrapper().CultureName = lang;
                }
                else
                {
                    viewModel2 = presenter.Save(viewModel);
                }
                return GetActionResult(ActionNames.Create, viewModel2);
            }
        }

        // GET: /Questions/Edit/5

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

        // POST: /Questions/Edit/5
        // POST: /Questions/Edit/5?lang=en-US

        [HttpPost]
        public ActionResult Edit(QuestionEditViewModel viewModel, string lang)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());

                object viewModel2;
                if (!String.IsNullOrEmpty(lang))
                {
                    viewModel2 = presenter.SetLanguage(viewModel, lang);
                    GetSessionWrapper().CultureName = lang;
                }
                else
                {
                    viewModel2 = presenter.Save(viewModel);
                }

                return GetActionResult(ActionNames.Edit, viewModel2);
            }
        }

        // GET: /Questions/Delete/5

        public ActionResult Delete(int id, string lang = null)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionConfirmDeletePresenter(repositories, TryGetAuthenticatedUserName());

                    if (!String.IsNullOrEmpty(lang))
                    {
                        viewModel = presenter.SetLanguage(id, lang);
                    }
                    else
                    {
                        viewModel = presenter.Show(id);
                    }
                }
            }

            return GetActionResult(ActionNames.Delete, viewModel);
        }

        // POST: /Questions/Delete/5

        [HttpPost]
        public ActionResult Delete(QuestionConfirmDeleteViewModel viewModel, int id, string lang = null)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);

                // Redirection goes wrong when you set language and do not do a custom handling.
                object viewModel2;
                if (!String.IsNullOrEmpty(lang))
                {
                    var presenter = new QuestionConfirmDeletePresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel2 = presenter.SetLanguage(viewModel, lang);
                    GetSessionWrapper().CultureName = lang;
                    TempData[TempDataKeys.ViewModel] = viewModel2;
                    return RedirectToAction(ActionNames.Delete, new { id });
                }
                else
                {
                    var presenter = new QuestionDeleteConfirmedPresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel2 = presenter.Show(id);
                }

                return GetActionResult(ActionNames.Delete, viewModel2);
            }
        }

        // POST: /Questions/AddLink

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

        // POST: /Questions/RemoveLink?temporaryID=12345678-90AB-CDEF

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

        // POST: /Questions/AddCategory

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

        // POST: /Questions/RemoveCategory?temporaryID=12345678-90AB-CDEF

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

        // GET: /Questions/Random
        // GET: /Questions/Random?c=1&c=2

        public ActionResult Random(int[] c)
        {
            object viewModel;
            if (!TempData.TryGetValue(TempDataKeys.ViewModel, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = CreateRandomQuestionPresenter(repositories);
                    viewModel = presenter.Show(c);
                }
            }

            return GetActionResult(ActionNames.Random, viewModel);
        }

        // POST: /Questions/Random?lang=en-US

        [HttpPost]
        public ActionResult Random(RandomQuestionViewModel viewModel, string lang)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.SetLanguage(viewModel, lang);
                GetSessionWrapper().CultureName = lang;
                return GetActionResult(ActionNames.Random, viewModel2);
            }
        }

        // POST: /Questions/ShowAnswer/5

        [HttpPost]
        public ActionResult ShowAnswer(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.ShowAnswer(viewModel);
                return GetActionResult(ActionNames.ShowAnswer, viewModel2);
            }
        }

        // POST: /Questions/HideAnswer/5

        [HttpPost]
        public ActionResult HideAnswer(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.HideAnswer(viewModel);
                return GetActionResult(ActionNames.HideAnswer, viewModel2);
            }
        }

        // POST: /Question/Flag/5

        [HttpPost]
        public ActionResult Flag(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = CreateRandomQuestionPresenter(repositories);
                object viewModel2 = presenter.Flag(viewModel);
                return GetActionResult(ActionNames.Flag, viewModel2);
            }
        }

        // POST: /Question/Unflag/5

        [HttpPost]
        public ActionResult Unflag(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Repositories repositories = PersistenceHelper.CreateRepositories(context);
                var presenter = CreateRandomQuestionPresenter(repositories);
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
