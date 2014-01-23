using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionDeletePresenter
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

        public QuestionDeletePresenter(
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

        /// <summary> Can return QuestionConfirmDeleteViewModel and QuestionNotFoundViewModel. </summary>
        public object Show(int id)
        {
            Question question = _questionRepository.TryGet(id);
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
            var deleteConfirmedPresenter = new QuestionDeleteConfirmedPresenter(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository, _sourceRepository, _questionTypeRepository);
            return deleteConfirmedPresenter.Show(id);
        }

        
        public PreviousViewModel Cancel()
        {
            return new PreviousViewModel();
        }
    }
}
