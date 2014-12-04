using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.Validation;
using JJ.Apps.QuestionAndAnswer.Resources;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionDetailsPresenter
    {
        private Repositories _repositories;

        public QuestionDetailsPresenter(Repositories repositories)
        {
            if (repositories == null) throw new ArgumentNullException("repositories");

            _repositories = repositories;
        }

        /// <summary> Can return QuestionDetailsViewModel or QuestionNotFoundViewModel. </summary>
        public object Show(int id)
        {
            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                return new QuestionNotFoundViewModel { ID = id };
            }

            QuestionDetailsViewModel viewModel = question.ToDetailsViewModel();
            return viewModel;
        }

        /// <summary> Can return QuestionEditViewModel or QuestionNotFoundViewModel. </summary>
        public object Edit(int id)
        {
            var editPresenter = new QuestionEditPresenter(_repositories);
            return editPresenter.Edit(id);
        }

        /// <summary> Can return QuestionConfirmDeleteViewModel and QuestionNotFoundViewModel. </summary>
        public object Delete(int id)
        {
            var deletePresenter = new QuestionDeletePresenter(_repositories);
            return deletePresenter.Show(id);
        }

        public QuestionListViewModel BackToList()
        {
            var listPresenter = new QuestionListPresenter(_repositories);
            return listPresenter.Show();
        }
    }
}
