using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionListPresenter
    {
        private IQuestionRepository _questionRepository;

        public QuestionListPresenter(IQuestionRepository questionRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");

            _questionRepository = questionRepository;
        }

        public QuestionListViewModel Show()
        {
            var listViewModel = new QuestionListViewModel();
            listViewModel.List = new List<QuestionViewModel>();

            foreach (Question question in _questionRepository.GetAll())
            {
                QuestionViewModel itemViewModel = question.ToViewModel();
                itemViewModel.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();
                listViewModel.List.Add(itemViewModel);
            }

            return listViewModel;
        }

        public QuestionListViewModel ShowByCriteria(bool? isFlagged)
        {
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
            IEnumerable<Question> questions = _questionRepository.GetByCriteria(mustFilterByFlagStatusID, flagStatusID);
            viewModel.List = questions.Select(x => x.ToViewModel()).ToArray();
            return viewModel;
        }
    }
}
