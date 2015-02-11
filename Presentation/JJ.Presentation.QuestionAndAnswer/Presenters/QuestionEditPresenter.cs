using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using JJ.Framework.Validation;
using JJ.Framework.Reflection;
using JJ.Framework.Business;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.SideEffects;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ToEntity;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.Resources;
using JJ.Presentation.QuestionAndAnswer.SideEffects;
using JJ.Framework.Presentation;
using System.Linq.Expressions;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionEditPresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionEditPresenter(
            Repositories repositories, 
            string authenticatedUserName)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _authenticatedUserName = authenticatedUserName;
        }

        public object Edit(int id)
        {
            if (String.IsNullOrEmpty(_authenticatedUserName))
            {
                var presenter2 = new LoginPresenter(_repositories);
                return presenter2.Show(CreateSourceAction(() => Edit(id)));
            }

            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter(_repositories.UserRepository, _authenticatedUserName);
                return presenter2.Show();
            }

            QuestionEditViewModel viewModel = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository, _repositories.UserRepository, _authenticatedUserName);
            viewModel.CanDelete = true;
            viewModel.Title = Titles.EditQuestion;

            return viewModel;
        }

        public object Create()
        {
            if (String.IsNullOrEmpty(_authenticatedUserName))
            {
                var presenter2 = new LoginPresenter(_repositories);
                return presenter2.Show(CreateSourceAction(() => Create()));
            }

            Question entity = _repositories.QuestionRepository.Create();
            _repositories.EntityStatusManager.SetIsNew(entity);

            ISideEffect sideEffect1 = new Question_SideEffect_AutoCreateRelatedEntities(entity, _repositories.AnswerRepository, _repositories.EntityStatusManager);
            sideEffect1.Execute();

            ISideEffect sideEffect2 = new Question_SideEffect_SetDefaults_ForOpenQuestion(entity, _repositories.QuestionTypeRepository, _repositories.EntityStatusManager);
            sideEffect2.Execute();

            ISideEffect sideEffect3 = new Question_SetDefaults_ForOpenQuestion_FrontEnd_SideEffect(entity, _repositories.SourceRepository, _repositories.EntityStatusManager);
            sideEffect3.Execute();

            QuestionEditViewModel viewModel = entity.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository, _repositories.UserRepository, _authenticatedUserName);
            viewModel.IsNew = true;
            viewModel.Title = Titles.CreateQuestion;

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
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository, _repositories.UserRepository, _authenticatedUserName);

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
                throw new Exception(String.Format("QuestionLinkViewModel with TemporaryID '{0}' not found.", temporaryID));
            }
            viewModel.Question.Links.Remove(questionLinkViewModel);

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository, _repositories.UserRepository, _authenticatedUserName);

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
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository, _repositories.UserRepository, _authenticatedUserName);

            // Non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel RemoveCategory(QuestionEditViewModel viewModel, Guid temporaryID)
        {
            // The problem here is that you may want to remove one out of many uncommitted entities do not exist in the database yet,
            // and you cannot identify them uniquely with the ID (which is 0),
            // which makes it impossible to perform the delete operation on the entity model when given an ID.
            // So instead you have to perform the operation on the viewmodel which has temporary ID's.

            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            // 'Business'
            QuestionCategoryViewModel questionCategoryViewModel = viewModel.Question.Categories.Where(x => x.TemporaryID == temporaryID).FirstOrDefault();
            if (questionCategoryViewModel == null)
            {
                throw new Exception(String.Format("questionCategoryViewModel with TemporaryID '{0}' not found.", temporaryID));
            }
            viewModel.Question.Categories.Remove(questionCategoryViewModel);

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository, _repositories.UserRepository, _authenticatedUserName);

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

            User user;
            if (String.IsNullOrEmpty(_authenticatedUserName))
            {
                return new NotAuthorizedViewModel();
            }
            else
            {
                user = _repositories.UserRepository.TryGetByUserName(_authenticatedUserName);
                if (user == null)
                {
                    return new NotAuthorizedViewModel();
                }
            }

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // Validate
            IValidator validator = new VersatileQuestionValidator(question);
            if (!validator.IsValid)
            {
                // ToViewModel
                QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository, _repositories.UserRepository, _authenticatedUserName);

                // Non-persisted properties
                viewModel2.IsNew = viewModel.IsNew;
                viewModel2.CanDelete = viewModel.CanDelete;
                viewModel2.Title = viewModel.Title;

                viewModel2.ValidationMessages = validator.ValidationMessages.ToCanonical();

                return viewModel2;
            }
            else
            {
                // Set Entity Status
                ViewModelEntityStatusHelper.SetPropertiesAreDirtyWithRelatedEntities(_repositories.EntityStatusManager, question, viewModel.Question);

                // Side-effects
                ISideEffect sideEffect1 = new Question_SideEffect_SetIsManual(question, _repositories.EntityStatusManager);
                sideEffect1.Execute();

                ISideEffect sideEffect2 = new Question_SideEffect_SetLastModifiedByUser(question, user, _repositories.EntityStatusManager);
                sideEffect2.Execute();

                foreach (QuestionFlag questionFlag in question.QuestionFlags)
                {
                    ISideEffect sideEffect3 = new QuestionFlag_SideEffect_SetLastModifiedByUser(questionFlag, user, _repositories.EntityStatusManager);
                    sideEffect3.Execute();
                }

                // Commit
                _repositories.QuestionRepository.Commit();

                // On success: go to Details view model.
                QuestionDetailsViewModel detailsViewModel = question.ToDetailsViewModel(_repositories.UserRepository, _authenticatedUserName);
                return detailsViewModel;
            }
        }

        public object Delete(QuestionEditViewModel viewModel)
        {
            var deletePresenter = new QuestionConfirmDeletePresenter(_repositories, _authenticatedUserName);
            return deletePresenter.Show(viewModel.Question.ID);
        }

        public QuestionListViewModel BackToList(int pageSize, int maxVisiblePageNumbers)
        {
            var listPresenter = new QuestionListPresenter(_repositories, _authenticatedUserName, pageSize, maxVisiblePageNumbers);
            return listPresenter.Show();
        }

        private ActionDescriptor CreateSourceAction(Expression<Func<object>> methodCallExpression)
        {
            return ActionDescriptorHelper.CreateActionDescriptor(GetType(), methodCallExpression);
        }
    }
}
