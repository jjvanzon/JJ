using System;
using System.Linq.Expressions;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Presentation;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class QuestionDeleteConfirmedPresenter
	{
		private readonly Repositories _repositories;
		private readonly string _authenticatedUserName;

		/// <param name="authenticatedUserName">nullable</param>
		public QuestionDeleteConfirmedPresenter(Repositories repositories, string authenticatedUserName)
		{
			_repositories = repositories ?? throw new NullException(() => repositories);
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