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
        private IAnswerRepository _answerRepository;
        private ICategoryRepository _categoryRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;
        private IQuestionFlagRepository _questionFlagRepository;
        private IFlagStatusRepository _flagStatusRepository;
        private ISourceRepository _sourceRepository;
        private IQuestionTypeRepository _questionTypeRepository;

        public QuestionListPresenter(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository,
            ISourceRepository sourceRepository,
            IQuestionTypeRepository questionTypeRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionCategoryRepository == null) throw new ArgumentNullException("questionCategoryRepository");
            if (questionLinkRepository == null) throw new ArgumentNullException("questionLinkRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (sourceRepository == null) throw new ArgumentNullException("sourceRepository");
            if (questionTypeRepository == null) throw new ArgumentNullException("questionTypeRepository");

            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _categoryRepository = categoryRepository;
            _questionCategoryRepository = questionCategoryRepository;
            _questionLinkRepository = questionLinkRepository;
            _questionFlagRepository = questionFlagRepository;
            _flagStatusRepository = flagStatusRepository;
            _sourceRepository = sourceRepository;
            _questionTypeRepository = questionTypeRepository;
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
            IEnumerable<Question> questions = _questionRepository.GetByCriteria(mustFilterByFlagStatusID, flagStatusID);
            viewModel.List = questions.Select(x => x.ToViewModel()).ToArray();
            return viewModel;
        }

        /// <summary> Can return QuestionDetailsViewModel or QuestionNotFoundViewModel. </summary>
        public object Details(int questionID)
        {
            var detailPresenter = new QuestionDetailsPresenter(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository, _sourceRepository, _questionTypeRepository);
            return detailPresenter.Show(questionID);
        }

        /// <summary> Can return QuestionDetailsViewModel or QuestionNotFoundViewModel. </summary>
        public object Edit(int questionID)
        {
            var editPresenter = new QuestionEditPresenter(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository, _sourceRepository, _questionTypeRepository);
            return editPresenter.Edit(questionID);
        }

        /// <summary> Can return QuestionDetailsViewModel or QuestionNotFoundViewModel. </summary>
        public object Delete(int questionID)
        {
            var deletePresenter = new QuestionDeletePresenter(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository, _sourceRepository, _questionTypeRepository);
            return deletePresenter.Show(questionID);
        }
    }
}
