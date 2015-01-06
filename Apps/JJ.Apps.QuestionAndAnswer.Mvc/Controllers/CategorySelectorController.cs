using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Common;
using JJ.Framework.Presentation.Mvc;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Models.QuestionAndAnswer.Repositories;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class CategorySelectorController : MasterController
    {
        // GET: /CategorySelector/

        public ActionResult Index()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                CategorySelectorRepositories repositories = CreateRepositories(context);
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                object viewModel = presenter.Show();
                return ViewPolymorphic(viewModel);
            }
        }

        // POST: /CategorySelector/Add/5

        [HttpPost]
        public ViewResult Add(int categoryID, CategorySelectorViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                CategorySelectorRepositories repositories = CreateRepositories(context);
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                object viewModel2 = presenter.Add(viewModel, categoryID);
                return ViewPolymorphic(viewModel2);
            }
        }

        // POST: /CategorySelector/AddCategory/5

        [HttpPost]
        public ViewResult Remove(int categoryID, CategorySelectorViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                CategorySelectorRepositories repositories = CreateRepositories(context);
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                object viewModel2 = presenter.Remove(viewModel, categoryID);
                return ViewPolymorphic(viewModel2);
            }
        }

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
