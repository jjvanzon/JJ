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
            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter();
                return presenter2.Show();
            }

            QuestionConfirmDeleteViewModel viewModel = question.ToConfirmDeleteViewModel();
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
    }
}
