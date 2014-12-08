using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionDeletePresenter
    {
        private Repositories _repositories;

        public QuestionDeletePresenter(Repositories repositories)
        {
            if (repositories == null) throw new ArgumentNullException("repositories");

            _repositories = repositories;
        }

        /// <summary> Can return QuestionConfirmDeleteViewModel and QuestionNotFoundViewModel. </summary>
        public object Show(int id)
        {
            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                return new QuestionNotFoundViewModel { ID = id };
            }

            QuestionConfirmDeleteViewModel viewModel = question.ToConfirmDeleteViewModel();
            return viewModel;
        }

        /// <summary> Can return QuestionDeleteConfirmedViewModel and QuestionNotFoundViewModel. </summary>
        public object ConfirmDelete(int id)
        {
            var deleteConfirmedPresenter = new QuestionDeleteConfirmedPresenter(_repositories);
            return deleteConfirmedPresenter.Show(id);
        }

        
        public PreviousViewModel Cancel()
        {
            return new PreviousViewModel();
        }
    }
}
