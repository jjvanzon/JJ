using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Apps.QuestionAndAnswer.ToEntity;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.Validation;
using JJ.Apps.QuestionAndAnswer.Resources;
using JJ.Framework.Reflection;
using JJ.Apps.QuestionAndAnswer.SideEffects;
using JJ.Framework.Business;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionEditPresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionEditPresenter(Repositories repositories, string authenticatedUserName)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _authenticatedUserName = authenticatedUserName;
        }

        /// <summary> Can return QuestionEditViewModel or QuestionNotFoundViewModel. </summary>
        public object Edit(int id)
        {
            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter();
                return presenter2.Show();
            }

            QuestionEditViewModel viewModel = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository);
            viewModel.CanDelete = true;
            viewModel.Title = Titles.EditQuestion;
            return viewModel;
        }

        public QuestionEditViewModel Create()
        {
            QuestionEditViewModel viewModel = ViewModelHelper.CreateEmptyQuestionEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository);
            viewModel.IsNew = true;
            viewModel.Title = Titles.CreateQuestion;

            ISideEffect setDefaults = new QuestionViewModel_SetDefaults_SideEffect(viewModel.Question, _repositories.SourceRepository, _repositories.QuestionTypeRepository);
            setDefaults.Execute();

            return viewModel;
        }

        public QuestionEditViewModel AddLink(QuestionEditViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoallesce();

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // Business
            QuestionLink questionLink = _repositories.QuestionLinkRepository.Create();
            questionLink.LinkTo(question);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository);

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
            viewModel.NullCoallesce();

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
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository);

            // Non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        public QuestionEditViewModel AddCategory(QuestionEditViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoallesce();

            // ToEntity
            Question question = viewModel.ToEntity(_repositories);

            // Businesss
            QuestionCategory questionCategory = _repositories.QuestionCategoryRepository.Create();
            questionCategory.LinkTo(question);

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository);

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
            viewModel.NullCoallesce();

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
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository);

            // Non-persisted properties
            viewModel2.IsNew = viewModel.IsNew;
            viewModel2.CanDelete = viewModel.CanDelete;
            viewModel2.Title = viewModel.Title;

            return viewModel2;
        }

        /// <summary> Can return QuestionEditViewModel or QuestionDetailsViewModel </summary>
        public object Save(QuestionEditViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoallesce();

            // Mark IsNew, IsDirty
            Question question = _repositories.QuestionRepository.TryGet(viewModel.Question.ID);
            viewModel.Question.SetIsDirtyRecursive(question);
            viewModel.Question.SetIsNewRecursive(question);

            // ToEntity
            question = viewModel.ToEntity(_repositories);

            // Side-effects
            ISideEffect setIsManual = new Question_SetIsManual_SideEffect(question, viewModel.Question);
            setIsManual.Execute();

            User user = _repositories.UserRepository.GetByUserName(_authenticatedUserName);
            ISideEffect setLastModifiedByUser = new Question_SetLastModifiedByUser_SideEffect(question, user, viewModel.Question);
            setLastModifiedByUser.Execute();

            foreach (QuestionFlagViewModel questionFlagViewModel in viewModel.Question.Flags)
            {
                QuestionFlag questionFlag = question.QuestionFlags.Where(x => x.ID == questionFlagViewModel.ID).Single();
                ISideEffect questionFlag_SetLastModifiedByUser = new QuestionFlag_SetLastModifiedByUser_SideEffect(questionFlag, user, questionFlagViewModel);
                questionFlag_SetLastModifiedByUser.Execute();
            }

            // ToViewModel
            QuestionEditViewModel viewModel2 = question.ToEditViewModel(_repositories.CategoryRepository, _repositories.FlagStatusRepository);

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
            _repositories.QuestionRepository.Commit();

            // On success: go to Details view model.
            QuestionDetailsViewModel detailsViewModel = question.ToDetailsViewModel();
            return detailsViewModel;
        }

        /// <summary> Can return QuestionConfirmDeleteViewModel and QuestionNotFoundViewModel. </summary>
        public object Delete(QuestionEditViewModel viewModel)
        {
            var deletePresenter = new QuestionConfirmDeletePresenter(_repositories, _authenticatedUserName);
            return deletePresenter.Show(viewModel.Question.ID);
        }

        public QuestionListViewModel BackToList()
        {
            var listPresenter = new QuestionListPresenter(_repositories, _authenticatedUserName);
            return listPresenter.Show();
        }
    }
}
