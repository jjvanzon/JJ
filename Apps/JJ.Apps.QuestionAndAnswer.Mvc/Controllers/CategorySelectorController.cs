using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Common;
using JJ.Framework.Presentation.Mvc;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.Mvc.Views;
using JJ.Apps.QuestionAndAnswer.Mvc.Views.Helpers;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class CategorySelectorController : MasterController
    {
        // GET: /CategorySelector/

        public ActionResult Index()
        {
            using (CategorySelectorRepositories repositories = CreateRepositories())
            {
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                CategorySelectorViewModel viewModel = presenter.Show();
                return View(viewModel);
            }
        }

        // POST: /CategorySelector/Add/5

        [HttpPost]
        public ViewResult Add(int categoryID, CategorySelectorViewModel viewModel)
        {
            using (CategorySelectorRepositories repositories = CreateRepositories())
            {
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                CategorySelectorViewModel viewModel2 = presenter.Add(viewModel, categoryID);
                return View(ViewNames.Index, viewModel2);
            }
        }

        // POST: /CategorySelector/AddCategory/5

        [HttpPost]
        public ViewResult Remove(int categoryID, CategorySelectorViewModel viewModel)
        {
            using (CategorySelectorRepositories repositories = CreateRepositories())
            {
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                CategorySelectorViewModel viewModel2 = presenter.Remove(viewModel, categoryID);
                return View(ViewNames.Index, viewModel2);
            }
        }

        private CategorySelectorPresenter CreatePresenter(CategorySelectorRepositories repositories)
        {
            return new CategorySelectorPresenter(
                repositories.CategoryRepository, 
                repositories.QuestionRepository, 
                repositories.QuestionFlagRepository, 
                repositories.FlagStatusRepository, 
                repositories.UserRepository);
        }

        private CategorySelectorRepositories CreateRepositories()
        {
            IContext context = ContextFactory.CreateContextFromConfiguration();

            return new CategorySelectorRepositories(
                new CategoryRepository(context),
                new QuestionRepository(context, context.Location),
                new QuestionFlagRepository(context),
                new FlagStatusRepository(context),
                new UserRepository(context),
                context);
        }
    }
}
