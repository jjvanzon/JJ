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
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters.Helpers;
using JJ.Apps.QuestionAndAnswer.Validation;
using JJ.Apps.QuestionAndAnswer.Resources;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionEditPresenter
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

        public QuestionEditPresenter(
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

        /// <summary> Can return QuestionEditViewModel or QuestionNotFoundViewModel. </summary>
        public object Edit(int id)
        {
            Question question = _questionRepository.TryGet(id);
            if (question == null)
            {
                return new QuestionNotFoundViewModel { ID = id };
            }

            QuestionEditViewModel viewModel = question.ToEditViewModel(_categoryRepository, _flagStatusRepository);
            viewModel.IsNew = false;
            viewModel.CanDelete = true;
            viewModel.Title = Titles.EditQuestion;
            return viewModel;
        }

        private const string DEFAULT_SOURCE_IDENTIFIER = "Manual";

        public QuestionEditViewModel Create()
        {
            QuestionEditViewModel viewModel = ViewModelHelper.CreateEmptyQuestionEditViewModel(_categoryRepository, _flagStatusRepository);
            viewModel.IsNew = true;
            viewModel.CanDelete = false;
            viewModel.Title = Titles.CreateQuestion;

            // These defaults are specific to the UI, not the business.
            Source source = _sourceRepository.GetByIdentifier(DEFAULT_SOURCE_IDENTIFIER);
            viewModel.Question.Source = source.ToViewModel();

            QuestionType questionType = _questionTypeRepository.Get((int)QuestionTypeEnum.OpenQuestion);
            viewModel.Question.Type = questionType.ToViewModel();

            return viewModel;
        }

        public QuestionEditViewModel AddLink(QuestionEditViewModel viewModel)
        {
            // If the question has disappeared, it is recreated.
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            // Get entity from database, with the viewmodel applied to it.
            Question question = ViewModelToEntity(viewModel);

            // Perform operation
            QuestionLink questionLink = _questionLinkRepository.Create();
            questionLink.LinkTo(question);

            // Create new view model
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_categoryRepository, _flagStatusRepository);

            // Copy non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel RemoveLink(QuestionEditViewModel viewModel, Guid temporaryID)
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
            Question question = ViewModelToEntity(viewModel);

            // Create new view model
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_categoryRepository, _flagStatusRepository);

            // Copy non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel AddCategory(QuestionEditViewModel viewModel)
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
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_categoryRepository, _flagStatusRepository);

            // Copy non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel RemoveCategory(QuestionEditViewModel viewModel, Guid temporaryID)
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
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_categoryRepository, _flagStatusRepository);

            // Copy non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        /// <summary> Can return QuestionEditViewModel or QuestionDetailsViewModel </summary>
        public object Save(QuestionEditViewModel viewModel)
        {
            // If the question has disappeared, it is recreated.
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            // Get entity from database, with the viewmodel applied to it.
            Question question = ViewModelToEntity(viewModel);

            // Produce new complete view model
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_categoryRepository, _flagStatusRepository);

            // Validate
            IValidator validator1 = new QuestionEditViewModelValidator(viewModel2);
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
            QuestionDetailsViewModel detailsViewModel = question.ToDetailsViewModel();
            return detailsViewModel;
        }

        public PreviousViewModel Cancel()
        {
            return new PreviousViewModel();
        }

        /// <summary> Can return QuestionConfirmDeleteViewModel and QuestionNotFoundViewModel. </summary>
        public object Delete(QuestionEditViewModel viewModel)
        {
            var deletePresenter = new QuestionDeletePresenter(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository, _sourceRepository, _questionTypeRepository);
            return deletePresenter.Show(viewModel.Question.ID);
        }

        public QuestionListViewModel BackToList()
        {
            var listPresenter = new QuestionListPresenter(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository, _sourceRepository, _questionTypeRepository);
            return listPresenter.Show();
        }

        private Question ViewModelToEntity(QuestionEditViewModel viewModel)
        {
            // Get entity from database, with the viewmodel applied to it.
            return viewModel.ToEntity(_questionRepository, _answerRepository, _categoryRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository, _flagStatusRepository, _sourceRepository, _questionTypeRepository);
        }
    }
}
