using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Common;
using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Persistence;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class CategorySelectorController : MasterController
    {
        public ActionResult Index()
        {
            object viewModel;
            if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    CategorySelectorRepositories repositories = CreateRepositories(context);
                    var presenter = CreatePresenter(repositories);
                    viewModel = presenter.Show();
                }
            }

            return ActionDispatcher.Dispatch(this, ActionNames.Index, viewModel);
        }

        [HttpPost]
        public ActionResult Add(CategorySelectorViewModel viewModel, int categoryID)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                CategorySelectorRepositories repositories = CreateRepositories(context);
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                object viewModel2 = presenter.Add(viewModel, categoryID);
                return ActionDispatcher.Dispatch(this, ActionNames.Add, viewModel2);
            }
        }

        [HttpPost]
        public ActionResult Remove(int categoryID, CategorySelectorViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                CategorySelectorRepositories repositories = CreateRepositories(context);
                var presenter = CreatePresenter(repositories);
                object viewModel2 = presenter.Remove(viewModel, categoryID);
                return ActionDispatcher.Dispatch(this, ActionNames.Remove, viewModel2);
            }
        }

        // Helpers

        private CategorySelectorPresenter CreatePresenter(CategorySelectorRepositories repositories)
        {
            return new CategorySelectorPresenter(
                repositories.CategoryRepository, 
                repositories.QuestionRepository, 
                repositories.QuestionFlagRepository, 
                repositories.FlagStatusRepository, 
                repositories.UserRepository,
                TryGetAuthenticatedUserName());
        }

        private CategorySelectorRepositories CreateRepositories(IContext context)
        {
            return new CategorySelectorRepositories(
                PersistenceHelper.CreateRepository<ICategoryRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionFlagRepository>(context),
                PersistenceHelper.CreateRepository<IFlagStatusRepository>(context),
                PersistenceHelper.CreateRepository<IUserRepository>(context));
        }
    }
}
