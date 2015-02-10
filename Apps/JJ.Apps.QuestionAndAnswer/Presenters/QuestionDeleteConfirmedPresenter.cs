using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Framework.Reflection;
using JJ.Framework.Presentation;
using System.Linq.Expressions;
using JJ.Apps.QuestionAndAnswer.ToViewModel;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionDeleteConfirmedPresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionDeleteConfirmedPresenter(
            Repositories repositories, 
            string authenticatedUserName)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _authenticatedUserName = authenticatedUserName;
        }
        
        public object Show(int id)
        {
            if (String.IsNullOrEmpty(_authenticatedUserName))
            {
                var presenter2 = new LoginPresenter(_repositories);
                return presenter2.Show(CreateSourceAction(() => Show(id)));
            }

            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter(_repositories.UserRepository, _authenticatedUserName);
                return presenter2.Show();
            }

            question.DeleteRelatedEntities(_repositories.AnswerRepository, _repositories.QuestionCategoryRepository, _repositories.QuestionLinkRepository, _repositories.QuestionFlagRepository);
            question.UnlinkRelatedEntities();

            QuestionDeleteConfirmedViewModel viewModel = question.ToDeleteConfirmedViewModel(_repositories.UserRepository, _authenticatedUserName);

            _repositories.QuestionRepository.Delete(question);
            _repositories.QuestionRepository.Commit();

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _repositories.UserRepository);

            return viewModel;
        }

        public QuestionListViewModel BackToList(int pageSize, int maxVisiblePageNumbers)
        {
            var listPresenter = new QuestionListPresenter(_repositories, _authenticatedUserName, pageSize, maxVisiblePageNumbers);
            return listPresenter.Show();
        }

        private ActionDescriptor CreateSourceAction(Expression<Func<object>> methodCallExpression)
        {
            return ActionDescriptorHelper.CreateActionDescriptor(GetType(), methodCallExpression);
        }
    }
}