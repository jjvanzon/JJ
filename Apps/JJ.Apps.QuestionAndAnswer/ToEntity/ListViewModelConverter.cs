using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Business;

namespace JJ.Apps.QuestionAndAnswer.ToEntity
{
    internal static class ListViewModelConverter
    {
        /// <summary> Returns newly created entities. </summary>
        public static IList<QuestionCategory> ConvertQuestionCategories(
            IList<QuestionCategoryViewModel> viewModels,
            IList<QuestionCategory> entities,
            IQuestionCategoryRepository questionCategoryRepository,
            ICategoryRepository categoryRepository,
            EntityStatusManager entityStatusManager)
        {
            var newEntities = new List<QuestionCategory>();

            foreach (QuestionCategoryViewModel viewModel in viewModels)
            {
                QuestionCategory entity = viewModel.ToEntity(questionCategoryRepository, categoryRepository, entityStatusManager);

                if (entityStatusManager.IsNew(entity))
                {
                    newEntities.Add(entity);
                }
            }

            // Delete
            ISet<int> entityIDs = new HashSet<int>(entities.Select(x => x.ID)
                                                           .Where(x => x != 0));

            ISet<int> viewModelIDs = new HashSet<int>(viewModels.Select(x => x.QuestionCategoryID)
                                                                .Where(x => x != 0));

            ISet<int> idsToDelete = new HashSet<int>(entityIDs.Except(viewModelIDs));

            foreach (QuestionCategory entity in entities.ToArray())
            {
                if (idsToDelete.Contains(entity.ID))
                {
                    entity.UnlinkRelatedEntities();
                    questionCategoryRepository.Delete(entity);
                }
            }

            return newEntities;
        }

        /// <summary> Returns newly created entities. </summary>
        public static IList<QuestionLink> ConvertQuestionLinks(
            IList<QuestionLinkViewModel> viewModels,
            IList<QuestionLink> entities,
            IQuestionLinkRepository questionLinkRepository,
            EntityStatusManager entityStatusManager)
        {
            var insertedEntities = new List<QuestionLink>();

            foreach (QuestionLinkViewModel viewModel in viewModels)
            {
                QuestionLink entity = viewModel.ToEntity(questionLinkRepository, entityStatusManager);

                if (entityStatusManager.IsNew(entity))
                {
                    insertedEntities.Add(entity);
                }
            }

            // Delete
            ISet<int> entityIDs = new HashSet<int>(entities.Select(x => x.ID)
                                                           .Where(x => x != 0));

            ISet<int> viewModelIDs = new HashSet<int>(viewModels.Select(x => x.ID)
                                                                .Where(x => x != 0));

            ISet<int> idsToDelete = new HashSet<int>(entityIDs.Except(viewModelIDs));

            foreach (QuestionLink entity in entities.ToArray())
            {
                if (idsToDelete.Contains(entity.ID))
                {
                    entity.UnlinkRelatedEntities();
                    questionLinkRepository.Delete(entity);
                }
            }

            return insertedEntities;
        }
    }
}
