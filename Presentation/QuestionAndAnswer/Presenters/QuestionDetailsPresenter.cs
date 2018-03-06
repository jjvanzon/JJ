using System;
using JJ.Data.QuestionAndAnswer;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Framework.Exceptions;
using System.Linq.Expressions;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Presentation;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class QuestionDetailsPresenter
	{
		private Repositories _repositories;
		private string _authenticatedUserName;

		/// <param name="authenticatedUserName">nullable</param>
		public QuestionDetailsPresenter(
			Repositories repositories, 
			string authenticatedUserName)
		{
			if (repositories == null) throw new NullException(() => repositories);

			_repositories = repositories;
			_authenticatedUserName = authenticatedUserName;
		}

		public object Show(int id)
		{
			Question question = _repositories.QuestionRepository.TryGet(id);
			if (question == null)
			{
				var presenter2 = new QuestionNotFoundPresenter(_repositories.UserRepository, _authenticatedUserName);
				return presenter2.Show();
			}

			QuestionDetailsViewModel viewModel = question.ToDetailsViewModel(_repositories.UserRepository, _authenticatedUserName);
			return viewModel;
		}

		public object Edit(int id)
		{
			var editPresenter = new QuestionEditPresenter(_repositories, _authenticatedUserName);
			return editPresenter.Edit(id, CreateReturnAction(() => Show(id)));
		}

		public object Delete(int id)
		{
			var deletePresenter = new QuestionConfirmDeletePresenter(_repositories, _authenticatedUserName);
			return deletePresenter.Show(id);
		}

		public object New()
		{
			var presenter2 = new QuestionEditPresenter(_repositories, _authenticatedUserName);
			return presenter2.Create();
		}

		public QuestionListViewModel BackToList()
		{
			var listPresenter = new QuestionListPresenter(_repositories, _authenticatedUserName);
			return listPresenter.Show();
		}

		private ActionInfo CreateReturnAction(Expression<Func<object>> methodCallExpression)
		{
			return ActionDispatcher.CreateActionInfo(GetType(), methodCallExpression);
		}
	}
}
