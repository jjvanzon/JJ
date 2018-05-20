using System.Web.Mvc;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;
using JJ.Framework.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

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

			return ActionDispatcher.Dispatch(this, nameof(ActionNames.Index), viewModel);
		}

		[HttpPost]
		public ActionResult Add(CategorySelectorViewModel viewModel, int categoryID)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				CategorySelectorRepositories repositories = CreateRepositories(context);
				CategorySelectorPresenter presenter = CreatePresenter(repositories);
				object viewModel2 = presenter.Add(viewModel, categoryID);
				return ActionDispatcher.Dispatch(this, nameof(ActionNames.Add), viewModel2);
			}
		}

		[HttpPost]
		public ActionResult Remove(int categoryID, CategorySelectorViewModel viewModel)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				CategorySelectorRepositories repositories = CreateRepositories(context);
				CategorySelectorPresenter presenter = CreatePresenter(repositories);
				object viewModel2 = presenter.Remove(viewModel, categoryID);
				return ActionDispatcher.Dispatch(this, nameof(ActionNames.Remove), viewModel2);
			}
		}

		// Helpers

		private CategorySelectorPresenter CreatePresenter(CategorySelectorRepositories repositories)
			=> new CategorySelectorPresenter(
				repositories.CategoryRepository,
				repositories.QuestionRepository,
				repositories.QuestionFlagRepository,
				repositories.FlagStatusRepository,
				repositories.UserRepository,
				TryGetAuthenticatedUserName());

		private CategorySelectorRepositories CreateRepositories(IContext context)
			=> new CategorySelectorRepositories(
				PersistenceHelper.CreateRepository<ICategoryRepository>(context),
				PersistenceHelper.CreateRepository<IQuestionRepository>(context),
				PersistenceHelper.CreateRepository<IQuestionFlagRepository>(context),
				PersistenceHelper.CreateRepository<IFlagStatusRepository>(context),
				PersistenceHelper.CreateRepository<IUserRepository>(context));
	}
}