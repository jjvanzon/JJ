using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Validation;
using JJ.Apps.QuestionAndAnswer.Presenters.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionDetailPresenter
    {
        private IQuestionRepository _questionRepository;
        private IAnswerRepository _answerRepository;
        private ICategoryRepository _categoryRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;
        private IQuestionFlagRepository _questionFlagRepository;
        private IFlagStatusRepository _flagStatusRepository;

        public QuestionDetailPresenter(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionCategoryRepository == null) throw new ArgumentNullException("questionCategoryRepository");
            if (questionLinkRepository == null) throw new ArgumentNullException("questionLinkRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");

            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _categoryRepository = categoryRepository;
            _questionCategoryRepository = questionCategoryRepository;
            _questionLinkRepository = questionLinkRepository;
            _questionFlagRepository = questionFlagRepository;
            _flagStatusRepository = flagStatusRepository;
        }

        /// <summary>
        /// Can return QuestionDetailViewModel or QuestionNotFoundViewModel.
        /// </summary>
        public object Show(int id)
        {
            Question question = _questionRepository.TryGet(id);
            if (question == null)
            {
                return new QuestionNotFoundViewModel { ID = id };
            }

            QuestionDetailViewModel viewModel = question.ToDetailViewModel(_flagStatusRepository);
            return viewModel;
        }

        public QuestionDetailViewModel AddLink(QuestionDetailViewModel viewModel)
        {
            // You can return QuestionDetailViewModel and never return QuestionNotFoundViewModel even though in high concurrency the question can disappear, because ToEntity recreates the question again.
            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }

            // Get entity from database, with the viewmodel applied to it.
            Question question = viewModel.ToEntity(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository);

            // Perform operation
            QuestionLink questionLink = _questionLinkRepository.Create();
            questionLink.LinkTo(question);

            // Create new view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository);
            return viewModel2;
        }

        public QuestionDetailViewModel RemoveLink(QuestionDetailViewModel viewModel, int questionLinkID, Guid questionLinkTemporaryID)
        {
            // You can return QuestionDetailViewModel and never return QuestionNotFoundViewModel even though in high concurrency the question can disappear, because ToEntity recreates the question again.

            // The problem here is that you may want to remove one out of many uncommitted entities do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0), which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So in that case you perform the operation on the viewmodel which has a temporary ID.

            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }
            if (viewModel.Question == null) { throw new ArgumentNullException("viewModel.Question"); }
            if (viewModel.Question.Links == null) { throw new ArgumentNullException("viewModel.Question.Links"); }

            // Perform operation on view model to remove an entity that does not exist in the database yet.
            if (questionLinkID == 0)
            {
                QuestionLinkViewModel questionLinkViewModel = viewModel.Question.Links.Where(x => x.TemporaryID == questionLinkTemporaryID).Single();
                viewModel.Question.Links.Remove(questionLinkViewModel);
            }

            // Get entity from database, with the viewmodel applied to it.
            Question question = viewModel.ToEntity(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository);

            // Perform operation on view model to remove an entity that did exist in the database.
            if (questionLinkID != 0)
            {
                QuestionLink questionLink = _questionLinkRepository.TryGet(questionLinkID);
                if (questionLink != null)
                {
                    questionLink.UnlinkRelatedEntities();
                    _questionLinkRepository.Delete(questionLink);
                }
            }

            // Create new view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository);
            return viewModel2;
        }

        public QuestionDetailViewModel Save(QuestionDetailViewModel viewModel)
        {
            // You can return QuestionDetailViewModel and never return QuestionNotFoundViewModel even though in high concurrency the question can disappear, because ToEntity recreates the question again.
            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }

            // Converts a partially filled view model to an entity.
            Question question = viewModel.ToEntity(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository);

            // Produce new complete view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository);

            // Validate
            IValidator validator = new QuestionValidator(question);
            if (!validator.IsValid)
            {
                viewModel2.ValidationMessages = validator.ValidationMessages.ToCanonical();
                return viewModel2;
            }

            // Commit
            _questionRepository.Commit();
            return viewModel2;
        }
    }
}
