using System;
using System.Linq;
using System.Linq.Expressions;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Business.QuestionAndAnswer.SideEffects;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Presentation;
using JJ.Framework.Resources;
using JJ.Framework.Validation;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.SideEffects;
using JJ.Presentation.QuestionAndAnswer.ToEntity;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionEditPresenter
    {
        private static readonly ActionInfo _defaultReturnAction;

        private readonly Repositories _repositories;
        private readonly string _authenticatedUserName;

        static QuestionEditPresenter() => _defaultReturnAction = ActionDispatcher.CreateActionInfo<QuestionListPresenter>(x => x.Show(1));

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionEditPresenter(
            Repositories repositories,
            string authenticatedUserName)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
            _authenticatedUserName = authenticatedUserName;
        }

        public object Edit(int id, ActionInfo returnAction = null)
        {
            returnAction = returnAction ?? _defaultReturnAction;

            if (string.IsNullOrEmpty(_authenticatedUserName))
            {
                var presenter2 = new LoginPresenter(_repositories);
                ActionInfo returnAction2 = CreateReturnAction(() => Edit(id, returnAction));
                return presenter2.Show(returnAction2);
            }

            Question question = _repositories.QuestionRepository.TryGet(id);

            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter(_repositories.UserRepository, _authenticatedUserName);
                return presenter2.Show();
            }

            if (returnAction == null)
            {
                returnAction = ActionDispatcher.CreateActionInfo<QuestionDetailsPresenter>(x => x.Show(id));
            }

            QuestionEditViewModel viewModel = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            viewModel.CanDelete = true;
            viewModel.Title = CommonResourceFormatter.Edit_WithName(ResourceFormatter.Question_Accusative);
            viewModel.ReturnAction = returnAction;

            return viewModel;
        }

        public object Create(ActionInfo returnAction = null)
        {
            returnAction = returnAction ?? _defaultReturnAction;

            if (string.IsNullOrEmpty(_authenticatedUserName))
            {
                var presenter2 = new LoginPresenter(_repositories);
                return presenter2.Show(CreateReturnAction(() => Create(null)));
            }

            Question entity = _repositories.QuestionRepository.Create();
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

            QuestionEditViewModel viewModel = entity.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            viewModel.IsNew = true;
            viewModel.Title = ResourceFormatter.CreateQuestion;

            if (returnAction == null)
            {
                returnAction = ActionDispatcher.CreateActionInfo<QuestionListPresenter>(x => x.Show(1));
            }

            viewModel.ReturnAction = returnAction;

            return viewModel;
        }

        public object AddLink(QuestionEditViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // Business
            QuestionLink questionLink = _repositories.QuestionLinkRepository.Create();
            questionLink.LinkTo(question);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // Non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel RemoveLink(QuestionEditViewModel viewModel, Guid temporaryID)
        {
            // The problem here is that you may want to remove one out of many uncommitted entities do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0),
            // which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So instead you have to perform the operation on the viewmodel which has temporary ID's.

            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            // 'Business'
            QuestionLinkViewModel questionLinkViewModel = viewModel.Question.Links.Where(x => x.TemporaryID == temporaryID).SingleOrDefault();

            if (questionLinkViewModel == null)
            {
                throw new Exception($"QuestionLinkViewModel with TemporaryID '{temporaryID}' not found.");
            }

            viewModel.Question.Links.Remove(questionLinkViewModel);

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // Non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel AddCategory(QuestionEditViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // Businesss
            QuestionCategory questionCategory = _repositories.QuestionCategoryRepository.Create();
            questionCategory.LinkTo(question);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // Non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel RemoveCategory(QuestionEditViewModel viewModel, Guid temporaryID)
        {
            // The problem here is that you may want to remove one out of many uncommitted entities that do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0),
            // which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So instead you have to perform the operation on the viewmodel which has temporary ID's.

            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            // 'Business'
            QuestionCategoryViewModel questionCategoryViewModel =
                viewModel.Question.Categories.Where(x => x.TemporaryID == temporaryID).FirstOrDefault();

            if (questionCategoryViewModel == null)
            {
                throw new Exception($"questionCategoryViewModel with TemporaryID '{temporaryID}' not found.");
            }

            viewModel.Question.Categories.Remove(questionCategoryViewModel);

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(
                _repositories.CategoryRepository,
                _repositories.UserRepository,
                _authenticatedUserName);

            // Non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public object Save(QuestionEditViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            if (string.IsNullOrEmpty(_authenticatedUserName))
            {
                return new NotAuthorizedViewModel();
            }

            User user = _repositories.UserRepository.TryGetByUserName(_authenticatedUserName);

            if (user == null)
            {
                return new NotAuthorizedViewModel();
            }

            // Set Entity Status (do this before ToEntity)
            Question question = _repositories.QuestionRepository.TryGet(viewModel.Question.ID);

            if (question != null)
            {
                ViewModelEntityStatusHelper.SetPropertiesAreDirtyWithRelatedEntities(
                    _repositories.EntityStatusManager,
                    question,
                    viewModel.Question);
            }

            // ToEntity
            question = viewModel.ToEntity(_repositories);

            // Validate
            IValidator validator = new VersatileQuestionValidator(question);

            if (!validator.IsValid)
            {
                // ToViewModel
                QuestionEditViewModel viewModel2 = question.ToEditViewModel(
                    _repositories.CategoryRepository,
                    _repositories.UserRepository,
                    _authenticatedUserName);

                // Non-persisted properties
                viewModel2.IsNew = viewModel.IsNew;
                viewModel2.CanDelete = viewModel.CanDelete;
                viewModel2.Title = viewModel.Title;

                viewModel2.ValidationMessages = validator.Messages;

                return viewModel2;
            }
            else
            {
                // Side-effects
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

                // On success: go to return action.
                ActionInfo returnAction = viewModel.ReturnAction ?? _defaultReturnAction;
                object viewModel2 = DispatchHelper.DispatchAction(returnAction, _repositories, _authenticatedUserName);
                return viewModel2;
            }
        }

        public object Cancel(QuestionEditViewModel viewModel)
        {
            ActionInfo returnAction = viewModel.ReturnAction ?? _defaultReturnAction;
            object viewModel2 = DispatchHelper.DispatchAction(returnAction, _repositories, _authenticatedUserName);
            return viewModel2;
        }

        private ActionInfo CreateReturnAction(Expression<Func<object>> methodCallExpression)
            => ActionDispatcher.CreateActionInfo(GetType(), methodCallExpression);
    }
}