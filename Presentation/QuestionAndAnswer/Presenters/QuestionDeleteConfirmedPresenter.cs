using System;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Framework.Exceptions;
using JJ.Framework.Presentation;
using System.Linq.Expressions;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class QuestionDeleteConfirmedPresenter
	{
		private Repositories _repositories;
		private string _authenticatedUserName;

		/// <param name="authenticatedUserName">nullable</param>
		public QuestionDeleteConfirmedPresenter(Repositories repositories, string authenticatedUserName)
		{
			if (repositories == null) throw new NullException(() => repositories);

			_repositories = repositories;
			_authenticatedUserName = authenticatedUserName;
		}
		
		public object Show(int id)
		{
			if (string.IsNullOrEmpty(_authenticatedUserName))
			{
				var presenter2 = new LoginPresenter(_repositories);
				return presenter2.Show(CreateReturnAction(() => Show(id)));
			}

			QuestionDeleteConfirmedViewModel viewModel = ViewModelHelper.CreateDeleteConfirmedViewModel(id, _repositories.UserRepository, _authenticatedUserName);
			return viewModel;
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