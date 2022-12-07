using System.Linq;
using JJ.Business.QuestionAndAnswer.Helpers;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_DeconstructionDeclarations

namespace JJ.Presentation.QuestionAndAnswer.Helpers
{
    /// <summary>
    /// Sets entity statuses that cannot always be set by the context or repository.
    /// This encompasses status dirty on individual properties and lists.
    /// </summary>
    internal static class ViewModelEntityStatusHelper
    {
        /// <param name="entity">nullable</param>
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

                if (EntityStatusHelper.GetListIsDirty(entity.QuestionCategories, x => x.ID, viewModel.Categories, x => x.QuestionCategoryID))
                {
                    statusManager.SetQuestionCategoriesListIsDirty(entity);

                    foreach (var (entity2, viewModel2) in entity.QuestionCategories.Zip(viewModel.Categories, (x, y) => (x, y)))
                    {
                        SetPropertiesAreDirty(statusManager, entity2, viewModel2);
                    }
                }

                if (EntityStatusHelper.GetListIsDirty(entity.QuestionLinks, x => x.ID, viewModel.Links, x => x.ID))
                {
                    statusManager.SetQuestionLinksListIsDirty(entity);

                    foreach (var (entity2, viewModel2) in entity.QuestionLinks.Zip(viewModel.Links, (x, y) => (x, y)))
                    {
                        SetPropertiesAreDirty(statusManager, entity2, viewModel2);
                    }
                }

                if (EntityStatusHelper.GetListIsDirty(entity.QuestionFlags, x => x.ID, viewModel.Flags, x => x.ID))
                {
                    statusManager.SetQuestionFlagsListIsDirty(entity);

                    foreach (var (entity2, viewModel2) in entity.QuestionFlags.Zip(viewModel.Flags, (x, y) => (x, y)))
                    {
                        SetPropertiesAreDirty(statusManager, entity2, viewModel2);
                    }
                }
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, Question entity, QuestionViewModel viewModel)
        {
            if (entity == null) return;

            if (viewModel.IsActive != entity.IsActive)
            {
                statusManager.SetIsActiveIsDirty(entity);
            }

            if (!string.Equals(viewModel.Text, entity.Text))
            {
                statusManager.SetTextIsDirty(entity);
            }

            if (viewModel.Type.ID != entity.QuestionType.ID)
            {
                statusManager.SetQuestionTypeIsDirty(entity);
            }

            if (viewModel.Source.ID != entity.Source.ID)
            {
                statusManager.SetSourceIsDirty(entity);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, Answer entity, QuestionViewModel viewModel)
        {
            if (entity == null) return;

            if (!string.Equals(viewModel.Answer, entity.Text))
            {
                statusManager.SetTextIsDirty(entity);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, QuestionCategory entity, QuestionCategoryViewModel viewModel)
        {
            if (entity == null) return;

            if (viewModel.Category.ID != entity.Category.ID)
            {
                statusManager.SetCategoryIsDirty(entity);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, QuestionLink entity, QuestionLinkViewModel viewModel)
        {
            if (entity == null) return;

            if (!string.Equals(viewModel.Url, entity.Url))
            {
                statusManager.SetUrlIsDirty(entity);
            }

            if (!string.Equals(viewModel.Description, entity.Description))
            {
                statusManager.SetDescriptionIsDirty(entity);
            }
        }

        private static void SetPropertiesAreDirty(EntityStatusManager statusManager, QuestionFlag entity, QuestionFlagViewModel viewModel)
        {
            if (entity == null) return;

            if (viewModel.Status.ID != entity.FlagStatus.ID)
            {
                statusManager.SetFlagStatusIsDirty(entity);
            }
        }
    }
}