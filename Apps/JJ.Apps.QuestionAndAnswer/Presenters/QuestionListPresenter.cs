using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Apps.QuestionAndAnswer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ToViewModel;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionListPresenter
    {
        private Repositories _repositories;

        public QuestionListPresenter(Repositories repositories)
        {
            if (repositories == null) throw new ArgumentNullException("repositories");

            _repositories = repositories;
        }

        public QuestionListViewModel Show()
        {
            var listViewModel = new QuestionListViewModel();
            listViewModel.List = new List<QuestionViewModel>();

            foreach (Question question in _repositories.QuestionRepository.GetAll())
            {
                QuestionViewModel itemViewModel = question.ToViewModel();
                itemViewModel.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();
                listViewModel.List.Add(itemViewModel);
            }

            return listViewModel;
        }

        public QuestionListViewModel ShowByCriteria(bool? isFlagged)
        {
            // TODO: We probably need more criteria.
            bool mustFilterByFlagStatusID = isFlagged.HasValue;
            int? flagStatusID = null;
            if (isFlagged.HasValue)
            {
                if (isFlagged.Value == true)
                {
                    flagStatusID = (int)FlagStatusEnum.Flagged;
                }
            }

            var viewModel = new QuestionListViewModel();
            IEnumerable<Question> questions = _repositories.QuestionRepository.GetByCriteria(mustFilterByFlagStatusID, flagStatusID);
            viewModel.List = questions.Select(x => x.ToViewModel()).ToArray();
            return viewModel;
        }

        /// <summary> Can return QuestionDetailsViewModel or QuestionNotFoundViewModel. </summary>
        public object Details(int questionID)
        {
            var detailPresenter = new QuestionDetailsPresenter(_repositories);
            return detailPresenter.Show(questionID);
        }

        /// <summary> Can return QuestionDetailsViewModel or QuestionNotFoundViewModel. </summary>
        public object Edit(int questionID)
        {
            var editPresenter = new QuestionEditPresenter(_repositories);
            return editPresenter.Edit(questionID);
        }

        /// <summary> Can return QuestionDetailsViewModel or QuestionNotFoundViewModel. </summary>
        public object Delete(int questionID)
        {
            var deletePresenter = new QuestionConfirmDeletePresenter(_repositories);
            return deletePresenter.Show(questionID);
        }
    }
}
