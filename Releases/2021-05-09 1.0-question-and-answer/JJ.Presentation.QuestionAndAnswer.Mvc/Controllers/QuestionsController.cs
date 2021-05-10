using System;
using System.Web.Mvc;
using JJ.Framework.Data;
using JJ.Framework.Web;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.Mvc.Helpers;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using ActionDispatcher = JJ.Presentation.QuestionAndAnswer.Mvc.Helpers.ActionDispatcher;
// ReSharper disable UnusedParameter.Global
// ReSharper disable InvertIf

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Controllers
{
	public class QuestionsController : MasterController
	{
		public QuestionsController() => ValidateRequest = false;
 
        // CRUD

		public ActionResult Index(int page = 1)
		{
			if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
			{
				using (IContext context = PersistenceHelper.CreateContext())
				{
					Repositories repositories = PersistenceHelper.CreateRepositories(context);
					var presenter = new QuestionListPresenter(repositories, TryGetAuthenticatedUserName());
					viewModel = presenter.Show(page);
				}
			}

			return ActionDispatcher.Dispatch(this, viewModel);
		}

		public ActionResult Details(int id)
		{
			if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
			{
				using (IContext context = PersistenceHelper.CreateContext())
				{
					Repositories repositories = PersistenceHelper.CreateRepositories(context);
					var presenter = new QuestionDetailsPresenter(repositories, TryGetAuthenticatedUserName());
					viewModel = presenter.Show(id);
				}
			}

			return ActionDispatcher.Dispatch(this, viewModel);
		}

		public ActionResult Create(string ret)
		{
		    if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
		    {
		        using (IContext context = PersistenceHelper.CreateContext())
		        {
		            Repositories repositories = PersistenceHelper.CreateRepositories(context);
		            var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
		            viewModel = presenter.Create(ret);
		        }
		    }

		    return ActionDispatcher.Dispatch(this, viewModel);
		}

		[HttpPost]
		public ActionResult Create(QuestionEditViewModel userInput, string ret)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
				QuestionEditViewModel viewModel = presenter.Save(userInput, ret);

                if (viewModel.Successful)
			    {
			        return Redirect(ret);
			    }

			    return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		public ActionResult Edit(int id, string ret)
		{
		    if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
		    {
		        using (IContext context = PersistenceHelper.CreateContext())
		        {
		            Repositories repositories = PersistenceHelper.CreateRepositories(context);
		            var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());

		            bool isNew = id == default;

		            if (isNew)
                    {
		                viewModel = presenter.Create(ret);
                    }
                    else
		            {
		                viewModel = presenter.Edit(id, ret);
		            }
		        }
		    }

		    return ActionDispatcher.Dispatch(this, viewModel);
		}

		[HttpPost]
		public ActionResult Edit(QuestionEditViewModel userInput, string ret)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
				QuestionEditViewModel viewModel = presenter.Save(userInput, ret);

			    if (viewModel.Successful)
			    {
			        return Redirect(ret);
			    }

                return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

	    public ActionResult Delete(int id)
		{
            if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
            {
                using (IContext context = PersistenceHelper.CreateContext())
                {
                    Repositories repositories = PersistenceHelper.CreateRepositories(context);
                    var presenter = new QuestionConfirmDeletePresenter(repositories, TryGetAuthenticatedUserName());
                    viewModel = presenter.Show(id);
                }
            }

            return ActionDispatcher.Dispatch(this, viewModel);
		}

		[HttpPost]
		public ActionResult Delete(QuestionConfirmDeleteViewModel userInput, int id)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				var presenter = new QuestionConfirmDeletePresenter(repositories, TryGetAuthenticatedUserName());
				QuestionDeleteConfirmedViewModel viewModel = presenter.Confirm(id);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

        // Sub-Actions

		[HttpPost]
		public ActionResult AddLink(QuestionEditViewModel userInput, string ret)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
				QuestionEditViewModel viewModel = presenter.AddLink(userInput, ret);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		[HttpPost]
		public ActionResult RemoveLink(QuestionEditViewModel userInput, Guid tempID, string ret)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
				QuestionEditViewModel viewModel = presenter.RemoveLink(userInput, tempID, ret);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		[HttpPost]
		public ActionResult AddCategory(QuestionEditViewModel userInput, string ret)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
				QuestionEditViewModel viewModel = presenter.AddCategory(userInput, ret);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		[HttpPost]
		public ActionResult RemoveCategory(QuestionEditViewModel userInput, Guid tempID, string ret)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				var presenter = new QuestionEditPresenter(repositories, TryGetAuthenticatedUserName());
				QuestionEditViewModel viewModel = presenter.RemoveCategory(userInput, tempID, ret);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

        // RandomQuestion

		public ActionResult Random(int[] c)
		{
			if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
			{
				using (IContext context = PersistenceHelper.CreateContext())
				{
					Repositories repositories = PersistenceHelper.CreateRepositories(context);
					RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
					viewModel = presenter.Show(c);
				}
			}

			return ActionDispatcher.Dispatch(this, viewModel);
		}

		[HttpPost]
		public ActionResult Random(RandomQuestionViewModel userInput, string lang)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
				RandomQuestionViewModel viewModel = presenter.SetLanguage(userInput, lang);
				CultureWebHelper.SetCultureCookie(ControllerContext.HttpContext, lang);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		[HttpPost]
		public ActionResult ShowAnswer(RandomQuestionViewModel userInput)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
				RandomQuestionViewModel viewModel = presenter.ShowAnswer(userInput);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		[HttpPost]
		public ActionResult HideAnswer(RandomQuestionViewModel userInput)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
				RandomQuestionViewModel viewModel = presenter.HideAnswer(userInput);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		[HttpPost]
		public ActionResult Flag(RandomQuestionViewModel userInput)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
				RandomQuestionViewModel viewModel = presenter.Flag(userInput);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		[HttpPost]
		public ActionResult Unflag(RandomQuestionViewModel userInput)
		{
			using (IContext context = PersistenceHelper.CreateContext())
			{
				Repositories repositories = PersistenceHelper.CreateRepositories(context);
				RandomQuestionPresenter presenter = CreateRandomQuestionPresenter(repositories);
				RandomQuestionViewModel viewModel = presenter.Unflag(userInput);
				return ActionDispatcher.Dispatch(this, viewModel);
			}
		}

		private RandomQuestionPresenter CreateRandomQuestionPresenter(Repositories repositories)
			=> new RandomQuestionPresenter(
				repositories.QuestionRepository,
				repositories.CategoryRepository,
				repositories.QuestionFlagRepository,
				repositories.FlagStatusRepository,
				repositories.UserRepository,
				TryGetAuthenticatedUserName());
	}
}