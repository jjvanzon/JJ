using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Common;
using JJ.Framework.Presentation.AspNetMvc4;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Framework.Persistence;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class CategorySelectorController : MasterController
    {
        // GET: /CategorySelector/

        public override ActionResult Index()
        {
            CategorySelectorPresenter presenter = CreatePresenter();
            CategorySelectorViewModel viewModel = presenter.Show();
            return View(viewModel);
        }

        // POST: /CategorySelector/Add/5

        [HttpPost]
        public ViewResult Add(int categoryID, CategorySelectorViewModel viewModel)
        {
            CategorySelectorPresenter presenter = CreatePresenter();
            CategorySelectorViewModel viewModel2 = presenter.Add(viewModel, categoryID);
            return View(ViewNames.Index, viewModel2);
        }

        // POST: /CategorySelector/AddCategory/5

        [HttpPost]
        public ViewResult Remove(int categoryID, CategorySelectorViewModel viewModel)
        {
            CategorySelectorPresenter presenter = CreatePresenter();
            CategorySelectorViewModel viewModel2 = presenter.Remove(viewModel, categoryID);
            return View(ViewNames.Index, viewModel2);
        }

        private CategorySelectorPresenter CreatePresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            ICategoryRepository categoryRepository = RepositoryFactory.CreateCategoryRepository(context);
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            IQuestionFlagRepository questionFlagRepository = RepositoryFactory.CreateQuestionFlagRepository(context);
            IFlagStatusRepository flagStatusRepository = RepositoryFactory.CreateFlagStatusRepository(context);
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            CategorySelectorPresenter presenter = new CategorySelectorPresenter(categoryRepository, questionRepository, questionFlagRepository, flagStatusRepository, userRepository);
            return presenter;
        }
    }
}
