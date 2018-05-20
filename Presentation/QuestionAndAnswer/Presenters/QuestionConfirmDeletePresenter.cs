using System;
using System.Linq.Expressions;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Presentation;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class QuestionConfirmDeletePresenter
	{
		private readonly Repositories _repositories;
		private readonly string _authenticatedUserName;

		/// <param name="authenticatedUserName">nullable</param>
		public QuestionConfirmDeletePresenter(
			Repositories repositories,
			string authenticatedUserName)
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

			Question question = _repositories.QuestionRepository.TryGet(id);
			if (question == null)
			{
				var presenter2 = new QuestionNotFoundPresenter(_repositories.UserRepository, _authenticatedUserName);
				return presenter2.Show();
			}

			QuestionConfirmDeleteViewModel viewModel = question.ToConfirmDeleteViewModel(_repositories.UserRepository, _authenticatedUserName);
			return viewModel;
		}

		public object Confirm(int id)
		{
			if (string.IsNullOrEmpty(_authenticatedUserName))
			{
				var presenter2 = new LoginPresenter(_repositories);
				return presenter2.Show(CreateReturnAction(() => Show(id)));
			}

			Question question = _repositories.QuestionRepository.TryGet(id);
			if (question == null)
			{
				var presenter2 = new QuestionNotFoundPresenter(_repositories.UserRepository, _authenticatedUserName);
				return presenter2.Show();
			}

			question.DeleteRelatedEntities(_repositories.AnswerRepository, _repositories.QuestionCategoryRepository, _repositories.QuestionLinkRepository, _repositories.QuestionFlagRepository);
			question.UnlinkRelatedEntities();

			_repositories.QuestionRepository.Delete(question);
			_repositories.QuestionRepository.Commit();

			var presenter3 = new QuestionDeleteConfirmedPresenter(_repositories, _authenticatedUserName);
			return presenter3.Show(id);
		}

		public PreviousViewModel Cancel()
		{
			return new PreviousViewModel();
		}

		private ActionInfo CreateReturnAction(Expression<Func<object>> methodCallExpression)
		{
			return ActionDispatcher.CreateActionInfo(GetType(), methodCallExpression);
		}
	}
}
