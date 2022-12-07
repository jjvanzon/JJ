using System;
using JJ.Business.QuestionAndAnswer.Helpers;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;

namespace JJ.Presentation.QuestionAndAnswer.ToEntity
{
    internal static class EntityViewModelExtensions
    {
        public static QuestionCategory ToEntity(
            this QuestionCategoryViewModel viewModel,
            IQuestionCategoryRepository questionCategoryRepository,
            ICategoryRepository categoryRepository,
            EntityStatusManager entityStatusManager)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (questionCategoryRepository == null) throw new ArgumentNullException(nameof(questionCategoryRepository));
            if (categoryRepository == null) throw new ArgumentNullException(nameof(categoryRepository));
            if (entityStatusManager == null) throw new ArgumentNullException(nameof(entityStatusManager));

            QuestionCategory entity = questionCategoryRepository.TryGet(viewModel.QuestionCategoryID);

            if (entity == null)
            {
                entity = questionCategoryRepository.Create();
                entityStatusManager.SetIsNew(entity);
            }

            Category category = categoryRepository.TryGet(viewModel.Category.ID);
            entity.LinkTo(category);

            return entity;
        }

        public static QuestionFlag ToEntity(
            this QuestionFlagViewModel viewModel,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository,
            EntityStatusManager entityStatusManager)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (questionFlagRepository == null) throw new ArgumentNullException(nameof(questionFlagRepository));
            if (flagStatusRepository == null) throw new ArgumentNullException(nameof(flagStatusRepository));
            if (entityStatusManager == null) throw new ArgumentNullException(nameof(entityStatusManager));

            QuestionFlag entity = questionFlagRepository.TryGet(viewModel.ID);

            if (entity == null)
            {
                entity = questionFlagRepository.Create();
                entityStatusManager.SetIsNew(entity);
            }

            entity.FlagStatus = flagStatusRepository.Get(viewModel.Status.ID);
            entity.Comment = viewModel.Comment;

            return entity;
        }

        public static QuestionLink ToEntity(
            this QuestionLinkViewModel viewModel,
            IQuestionLinkRepository repository,
            EntityStatusManager entityStatusManager)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (entityStatusManager == null) throw new ArgumentNullException(nameof(entityStatusManager));

            QuestionLink entity = repository.TryGet(viewModel.ID);

            if (entity == null)
            {
                entity = repository.Create();
                entityStatusManager.SetIsNew(entity);
            }

            entity.Url = viewModel.Url;
            entity.Description = viewModel.Description;

            return entity;
        }
    }
}