using System.Web.Mvc;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using ActionDispatcher = JJ.Presentation.QuestionAndAnswer.Mvc.Helpers.ActionDispatcher;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
    public class CategorySelectorController : MasterController
    {
        public ActionResult Index()
        {
            if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    CategorySelectorRepositories repositories = CreateRepositories(context);
                    CategorySelectorPresenter presenter = CreatePresenter(repositories);
                    viewModel = presenter.Show();
                }
            }

            return ActionDispatcher.Dispatch(this, viewModel);
        }

        [HttpPost]
        public ActionResult Add(CategorySelectorViewModel userInput, int categoryID)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                CategorySelectorRepositories repositories = CreateRepositories(context);
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                CategorySelectorViewModel viewModel = presenter.Add(userInput, categoryID);
                return ActionDispatcher.Dispatch(this, viewModel);
            }
        }

        [HttpPost]
        public ActionResult Remove(CategorySelectorViewModel userInput, int categoryID)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                CategorySelectorRepositories repositories = CreateRepositories(context);
                CategorySelectorPresenter presenter = CreatePresenter(repositories);
                CategorySelectorViewModel viewModel = presenter.Remove(userInput, categoryID);
                return ActionDispatcher.Dispatch(this, viewModel);
            }
        }

        // Helpers

        private CategorySelectorPresenter CreatePresenter(CategorySelectorRepositories repositories)
            => new CategorySelectorPresenter(
                repositories.CategoryRepository,
                repositories.UserRepository,
                TryGetAuthenticatedUserName());

        private CategorySelectorRepositories CreateRepositories(IContext context)
            => new CategorySelectorRepositories(
                PersistenceHelper.CreateRepository<ICategoryRepository>(context),
                PersistenceHelper.CreateRepository<IUserRepository>(context));
    }
}