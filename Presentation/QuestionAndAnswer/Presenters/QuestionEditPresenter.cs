using System;
using System.Linq;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Business.QuestionAndAnswer.SideEffects;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Collections;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Resources;
using JJ.Framework.Validation;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.SideEffects;
using JJ.Presentation.QuestionAndAnswer.ToEntity;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;

// ReSharper disable RedundantIfElseBlock

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionEditPresenter
    {
        private readonly Repositories _repositories;
        private readonly SecurityAsserter _securityAsserter;
        private readonly string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionEditPresenter(Repositories repositories, string authenticatedUserName)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
            _securityAsserter = new SecurityAsserter(repositories.UserRepository);
            _authenticatedUserName = authenticatedUserName;
        }

        public QuestionEditViewModel Edit(int id, string returnAction)
        {
            if (string.IsNullOrEmpty(returnAction)) throw new NullOrEmptyException(nameof(returnAction));

            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // GetEntity
            Question question = _repositories.QuestionRepository.Get(id);

            // ToViewModel
            QuestionEditViewModel viewModel = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // NonPersisted
            viewModel.CanDelete = true;
            viewModel.Title = CommonResourceFormatter.Edit_WithName(ResourceFormatter.Question_Accusative);
            SetReturnAction(viewModel, returnAction);

            return viewModel;
        }

        public QuestionEditViewModel Create(string returnAction)
        {
            if (string.IsNullOrEmpty(returnAction)) throw new NullOrEmptyException(nameof(returnAction));
        
            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // GetEntity
            Question entity = _repositories.QuestionRepository.Create();

            // Business
            _repositories.EntityStatusManager.SetIsNew(entity);

            ISideEffect sideEffect1 = new Question_SideEffect_AutoCreateRelatedEntities(
                entity,
                _repositories.AnswerRepository,
                _repositories.EntityStatusManager);

            sideEffect1.Execute();

            ISideEffect sideEffect2 = new Question_SideEffect_SetDefaults_ForOpenQuestion(
                entity,
                _repositories.QuestionTypeRepository,
                _repositories.EntityStatusManager);

            sideEffect2.Execute();

            ISideEffect sideEffect3 = new Question_SetDefaults_ForOpenQuestion_FrontEnd_SideEffect(
                entity,
                _repositories.SourceRepository,
                _repositories.EntityStatusManager);

            sideEffect3.Execute();

            // ToViewModel
            QuestionEditViewModel viewModel = entity.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // NonPersisted
            viewModel.IsNew = true;
            viewModel.Title = ResourceFormatter.CreateQuestion;
            SetReturnAction(viewModel, returnAction);

            return viewModel;
        }

        public QuestionEditViewModel AddLink(QuestionEditViewModel userInput, string returnAction)
        {
            if (userInput == null) throw new NullException(() => userInput);
            userInput.NullCoalesce();

            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // ToEntity
            Question question = userInput.ToEntity(_repositories);

            // Business
            QuestionLink questionLink = _repositories.QuestionLinkRepository.Create();
            questionLink.LinkTo(question);

            // ToViewModel
            QuestionEditViewModel viewModel = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // NonPersisted
            CopyNonPersistedProperties(userInput, viewModel);
            SetReturnAction(viewModel, returnAction);

            return viewModel;
        }

        public QuestionEditViewModel RemoveLink(QuestionEditViewModel userInput, Guid temporaryID, string returnAction)
        {
            // The problem here is that you may want to remove one out of many uncommitted entities do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0),
            // which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So instead you have to perform the operation on the viewmodel which has temporary ID's.

            if (userInput == null) throw new NullException(() => userInput);
            if (string.IsNullOrEmpty(returnAction)) throw new NullOrEmptyException(nameof(returnAction));
            userInput.NullCoalesce();

            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // 'Business'
            QuestionLinkViewModel questionLinkViewModel = userInput.Question.Links.Where(x => x.TemporaryID == temporaryID).SingleOrDefault();

            if (questionLinkViewModel == null)
            {
                throw new Exception($"QuestionLinkViewModel with TemporaryID '{temporaryID}' not found.");
            }

            userInput.Question.Links.Remove(questionLinkViewModel);

            // ToEntity
            Question question = userInput.ToEntity(_repositories);

            // ToViewModel
            QuestionEditViewModel viewModel = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // NonPersisted
            CopyNonPersistedProperties(userInput, viewModel);
            SetReturnAction(viewModel, returnAction);

            return viewModel;
        }

        public QuestionEditViewModel AddCategory(QuestionEditViewModel userInput, string returnAction)
        {
            if (userInput == null) throw new NullException(() => userInput);
            if (string.IsNullOrEmpty(returnAction)) throw new NullOrEmptyException(nameof(returnAction));
            userInput.NullCoalesce();

            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // ToEntity
            Question question = userInput.ToEntity(_repositories);

            // Businesss
            QuestionCategory questionCategory = _repositories.QuestionCategoryRepository.Create();
            questionCategory.LinkTo(question);

            // ToViewModel
            QuestionEditViewModel viewModel = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // NonPersisted
            CopyNonPersistedProperties(userInput, viewModel);
            SetReturnAction(viewModel, returnAction);

            return viewModel;
        }

        public QuestionEditViewModel RemoveCategory(QuestionEditViewModel userInput, Guid temporaryID, string returnAction)
        {
            // The problem here is that you may want to remove one out of many uncommitted entities that do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0),
            // which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So instead you have to perform the operation on the viewmodel which has temporary ID's.

            if (userInput == null) throw new NullException(() => userInput);
            if (string.IsNullOrEmpty(returnAction)) throw new NullOrEmptyException(nameof(returnAction));
            userInput.NullCoalesce();

            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // 'Business'
            QuestionCategoryViewModel questionCategoryViewModel =
                userInput.Question.Categories.Where(x => x.TemporaryID == temporaryID).FirstOrDefault();

            if (questionCategoryViewModel == null)
            {
                throw new Exception($"questionCategoryViewModel with TemporaryID '{temporaryID}' not found.");
            }

            userInput.Question.Categories.Remove(questionCategoryViewModel);

            // ToEntity
            Question question = userInput.ToEntity(_repositories);

            // ToViewModel
            QuestionEditViewModel viewModel = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // NonPersisted
            CopyNonPersistedProperties(userInput, viewModel);
            SetReturnAction(viewModel, returnAction);

            return viewModel;
        }

        public QuestionEditViewModel Save(QuestionEditViewModel userInput, string returnAction)
        {
            if (userInput == null) throw new NullException(() => userInput);
            if (string.IsNullOrEmpty(returnAction)) throw new NullOrEmptyException(nameof(returnAction));
            userInput.NullCoalesce();

            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // Set Entity Status (do this before ToEntity)
            Question question = _repositories.QuestionRepository.TryGet(userInput.Question.ID);

            if (question != null)
            {
                ViewModelEntityStatusHelper.SetPropertiesAreDirtyWithRelatedEntities(
                    _repositories.EntityStatusManager,
                    question,
                    userInput.Question);
            }

            // GetEntity / ToEntity
            User user = _repositories.UserRepository.GetByUserName(_authenticatedUserName);
            question = userInput.ToEntity(_repositories);

            // Validate
            IValidator validator = new VersatileQuestionValidator(question);

            if (!validator.IsValid)
            {
                // ToViewModel
                QuestionEditViewModel viewModel = question.ToEditViewModel(
                    _repositories.CategoryRepository,
                    _repositories.UserRepository,
                    _authenticatedUserName);

                // NonPersisted
                CopyNonPersistedProperties(userInput, viewModel);
                SetReturnAction(viewModel, returnAction);
                viewModel.ValidationMessages = validator.Messages;

                return viewModel;
            }
            else
            {
                // SideEffects
                ISideEffect sideEffect1 = new Question_SideEffect_SetIsManual(question, _repositories.EntityStatusManager);
                sideEffect1.Execute();

                ISideEffect sideEffect2 = new Question_SideEffect_SetLastModifiedByUser(question, user, _repositories.EntityStatusManager);
                sideEffect2.Execute();

                foreach (QuestionFlag questionFlag in question.QuestionFlags)
                {
                    ISideEffect sideEffect3 = new QuestionFlag_SideEffect_SetLastModifiedByUser(
                        questionFlag,
                        user,
                        _repositories.EntityStatusManager);

                    sideEffect3.Execute();
                }

                // Commit
                _repositories.QuestionRepository.Commit();

                // NonPersisted
                userInput.Successful = true;

                return userInput;
            }
        }

        private static void CopyNonPersistedProperties(QuestionEditViewModel source, QuestionEditViewModel dest)
        {
            dest.IsNew = source.IsNew;
            dest.CanDelete = source.CanDelete;
            dest.Title = source.Title;
        }

        private static void SetReturnAction(QuestionEditViewModel viewModel, string returnAction)
        {
            if (string.IsNullOrEmpty(returnAction)) throw new NullOrEmptyException(nameof(returnAction));

            viewModel.ReturnAction = returnAction;
            viewModel.Question.ReturnAction = returnAction;
            viewModel.Question.Categories.ForEach(x => x.ReturnAction = returnAction);
            viewModel.Question.Links.ForEach(x => x.ReturnUrl = returnAction);
        }
    }
}