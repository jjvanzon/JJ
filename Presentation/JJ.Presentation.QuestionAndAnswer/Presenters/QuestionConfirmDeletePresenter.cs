using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection;
using System.Linq.Expressions;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionConfirmDeletePresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionConfirmDeletePresenter(
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

            QuestionConfirmDeleteViewModel viewModel = question.ToConfirmDeleteViewModel(_repositories.UserRepository, _authenticatedUserName);
            return viewModel;
        }

        public object Confirm(int id)
        {
            var presenter2 = new QuestionDeleteConfirmedPresenter(_repositories, _authenticatedUserName);
            return presenter2.Show(id);
        }
        
        public PreviousViewModel Cancel()
        {
            return new PreviousViewModel();
        }

        private ActionDescriptor CreateSourceAction(Expression<Func<object>> methodCallExpression)
        {
            return ActionDescriptorHelper.CreateActionDescriptor(GetType(), methodCallExpression);
        }
    }
}
