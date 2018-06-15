using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
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
            IFlagStatusRepository flagStatusRepository)
        {
            QuestionFlag questionFlag = questionFlagRepository.TryGet(viewModel.ID);

            // TODO: Low prio: This is not the TryGet-Insert-Update pattern. It just happens to work for the QuestionEditViewModel.
            if (questionFlag != null)
            {
                questionFlag.FlagStatus = flagStatusRepository.Get(viewModel.Status.ID);
                questionFlag.Comment = viewModel.Comment;
            }

            return questionFlag;
        }

        public static QuestionLink ToEntity(
            this QuestionLinkViewModel viewModel,
            IQuestionLinkRepository repository,
            EntityStatusManager entityStatusManager)
        {
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