using JJ.Framework.Business;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Data.QuestionAndAnswer;
using System;
using JJ.Framework.Exceptions;

namespace JJ.Presentation.QuestionAndAnswer.Helpers
{
    /// <summary>
    /// Sets entity statuses that cannot always be set by the context or repository.
    /// This encompasses status dirty on individual properties and lists.
    /// </summary>
    internal static class ViewModelEntityStatusHelper
    {
        /// <param name="entity"> nullable </param>
        public static void SetPropertiesAreDirtyWithRelatedEntities(EntityStatusManager statusManager, Question entity, QuestionViewModel viewModel)
        {
            if (statusManager == null) throw new NullException(() => statusManager);
            if (viewModel == null) throw new NullException(() => viewModel);

            SetPropertiesAreDirty(statusManager, entity, viewModel);

            if (entity != null)
            {
                if (entity.Answers.Count > 0)
                {
                    SetPropertiesAreDirty(statusManager, entity.Answers[0], viewModel);
                }

                if (EntityStatusHelper.GetListIsDirty(viewModel.Categories, x => x.QuestionCategoryID, entity.QuestionCategories, x => x.ID))
                {
                    statusManager.SetIsDirty(() => entity.QuestionCategories);
                }

                if (EntityStatusHelper.GetListIsDirty(viewModel.Links, x => x.ID, entity.QuestionLinks, x => x.ID))
                {
                    statusManager.SetIsDirty(() => entity.QuestionLinks);
                }

                if (EntityStatusHelper.GetListIsDirty(viewModel.Flags, x => x.ID, entity.QuestionFlags, x => x.ID))
                {
                    statusManager.SetIsDirty(() => entity.QuestionFlags);
                }
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, Question entity, QuestionViewModel viewModel)
        {
            if (entity == null) return;

            if (viewModel.IsActive != entity.IsActive)
            {
                statusManager.SetIsDirty(() => entity.IsActive);
            }

            if (!string.Equals(viewModel.Text, entity.Text))
            {
                statusManager.SetIsDirty(() => entity.Text);
            }

            if (viewModel.Type.ID != entity.QuestionType.ID)
            {
                statusManager.SetIsDirty(() => entity.QuestionType);
            }

            if (viewModel.Source.ID != entity.Source.ID)
            {
                statusManager.SetIsDirty(() => entity.Source);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, Answer entity, QuestionViewModel viewModel)
        {
            if (entity == null) return;

            if (!string.Equals(viewModel.Answer, entity.Text))
            {
                statusManager.SetIsDirty(() => entity.Text);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, QuestionCategory entity, QuestionCategoryViewModel viewModel)
        {
            if (entity == null) return;

            if (viewModel.Category.ID != entity.Category.ID)
            {
                statusManager.SetIsDirty(() => entity.Category);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, QuestionLink entity, QuestionLinkViewModel viewModel)
        {
            if (entity == null) return;

            if (!string.Equals(viewModel.Url, entity.Url))
            {
                statusManager.SetIsDirty(() => entity.Url);
            }

            if (!string.Equals(viewModel.Description, entity.Description))
            {
                statusManager.SetIsDirty(() => entity.Description);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, QuestionFlag entity, QuestionFlagViewModel viewModel)
        {
            if (entity == null) return;

            if (viewModel.Status.ID != entity.FlagStatus.ID)
            {
                statusManager.SetIsDirty(() => entity.FlagStatus);
            }
        }
    }
}
