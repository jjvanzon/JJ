using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection;
using System.Linq.Expressions;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionConfirmDeletePresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionConfirmDeletePresenter(Repositories repositories, string authenticatedUserName)
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
                var presenter2 = new QuestionNotFoundPresenter(_authenticatedUserName, _repositories.UserRepository);
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

        public object SetLanguage(QuestionConfirmDeleteViewModel viewModel, string cultureName)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();
            return Show(viewModel.ID);
        }

        private ActionDescriptor CreateSourceAction(Expression<Func<object>> methodCallExpression)
        {
            return ActionDescriptorHelper.CreateActionDescriptor(GetType(), methodCallExpression);
        }
    }
}
