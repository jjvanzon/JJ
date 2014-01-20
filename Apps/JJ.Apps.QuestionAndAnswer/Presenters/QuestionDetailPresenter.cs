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
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters.Helpers;
using JJ.Apps.QuestionAndAnswer.Validation;

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

            QuestionDetailViewModel viewModel = question.ToDetailViewModel(_flagStatusRepository, _categoryRepository);
            return viewModel;
        }

        public QuestionDetailViewModel AddLink(QuestionDetailViewModel viewModel)
        {
            // If the question has disappeared, it is recreated.
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            // Get entity from database, with the viewmodel applied to it.
            Question question = viewModel.ToEntity(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository);

            // Perform operation
            QuestionLink questionLink = _questionLinkRepository.Create();
            questionLink.LinkTo(question);

            // Create new view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository, _categoryRepository);
            return viewModel2;
        }

        public QuestionDetailViewModel RemoveLink(QuestionDetailViewModel viewModel, Guid temporaryID)
        {
            // If the question has disappeared, it is recreated.

            // The problem here is that you may want to remove one out of many uncommitted entities do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0),
            // which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So instead you have to perform the operation on the viewmodel which has temporary ID's.

            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            // Perform operation
            QuestionLinkViewModel questionLinkViewModel = viewModel.Question.Links.Where(x => x.TemporaryID == temporaryID).SingleOrDefault();
            if (questionLinkViewModel == null)
            {
                throw new Exception(String.Format("QuestionLinkViewModel with TemporaryID '{0}' not found.", temporaryID));
            }
            viewModel.Question.Links.Remove(questionLinkViewModel);

            // Get entity from database, with the viewmodel applied to it.
            Question question = viewModel.ToEntity(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository);

            // Create new view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository, _categoryRepository);
            return viewModel2;
        }

        public QuestionDetailViewModel AddCategory(QuestionDetailViewModel viewModel)
        {
            // If the question has disappeared, it is recreated.
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            // Get entity from database, with the viewmodel applied to it.
            Question question = ViewModelToEntity(viewModel);

            // Perform operation
            QuestionCategory questionCategory = _questionCategoryRepository.Create();
            questionCategory.LinkTo(question);

            // Create new view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository, _categoryRepository);
            return viewModel2;
        }

        public QuestionDetailViewModel RemoveCategory(QuestionDetailViewModel viewModel, Guid temporaryID)
        {
            // If the question has disappeared, it is recreated.

            // The problem here is that you may want to remove one out of many uncommitted entities do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0),
            // which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So instead you have to perform the operation on the viewmodel which has temporary ID's.

            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            // Perform operation
            QuestionCategoryViewModel questionCategoryViewModel = viewModel.Question.Categories.Where(x => x.TemporaryID == temporaryID).FirstOrDefault();
            if (questionCategoryViewModel == null)
            {
                throw new Exception(String.Format("questionCategoryViewModel with TemporaryID '{0}' not found.", temporaryID));
            }
            viewModel.Question.Categories.Remove(questionCategoryViewModel);

            // Get entity from database, with the viewmodel applied to it.
            Question question = ViewModelToEntity(viewModel);

            // Create new view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository, _categoryRepository);
            return viewModel2;
        }

        public QuestionDetailViewModel Save(QuestionDetailViewModel viewModel)
        {
            // If the question has disappeared, it is recreated.
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            // Get entity from database, with the viewmodel applied to it.
            Question question = ViewModelToEntity(viewModel);

            // Produce new complete view model
            QuestionDetailViewModel viewModel2 = question.ToDetailViewModel(_flagStatusRepository, _categoryRepository);

            // Validate
            IValidator validator1 = new QuestionDetailViewModelValidator(viewModel2);
            if (!validator1.IsValid)
            {
                viewModel2.ValidationMessages = validator1.ValidationMessages.ToCanonical();
                return viewModel2;
            }

            IValidator validator2 = new QuestionValidator(question);
            if (!validator2.IsValid)
            {
                viewModel2.ValidationMessages = validator2.ValidationMessages.ToCanonical();
                return viewModel2;
            }

            // Commit
            _questionRepository.Commit();
            return viewModel2;
        }

        private Question ViewModelToEntity(QuestionDetailViewModel viewModel)
        {
            // Get entity from database, with the viewmodel applied to it.
            return viewModel.ToEntity(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository);
        }
    }
}
