using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class ViewModelExtensions_IsDirty
    {
        public static bool GetIsDirty(this QuestionViewModel viewModel, Question entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (entity == null) throw new ArgumentNullException("entity");

            viewModel.NullCoallesce();

            return viewModel.Answer != entity.Answers[0].Text || // TODO: Support multiple answers.
                   viewModel.IsActive != entity.IsActive ||
                   viewModel.Text != entity.Text ||
                   viewModel.Type.ID != entity.QuestionType.ID ||
                   viewModel.Source.ID != entity.Source.ID;
        }
        
        public static bool GetIsDirty(this QuestionCategoryViewModel viewModel, QuestionCategory entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (entity == null) throw new ArgumentNullException("entity");

            viewModel.NullCoallesce();

            return viewModel.Category.ID != entity.Category.ID;
        }

        public static bool GetIsDirty(this QuestionLinkViewModel viewModel, QuestionLink entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (entity == null) throw new ArgumentNullException("entity");

            return viewModel.Url != entity.Url ||
                   viewModel.Description != entity.Description;
        }

        public static bool IsDirty(QuestionFlagViewModel viewModel, QuestionFlag entity)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (entity == null) throw new ArgumentNullException("entity");

            viewModel.NullCoallesce();

            return entity.FlagStatus.ID != viewModel.Status.ID;
        }

        /// <summary>
        /// Marks different pieces of the view model as dirty,
        /// by setting IsDirty properties in the view model,
        /// depending on the differences with the passed entity.
        /// </summary>
        /// <returns>
        /// whether anything was dirty.
        /// </returns>
        public static bool SetIsDirtyRecursive(this QuestionViewModel viewModel, Question question)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.NullCoallesce();

            viewModel.IsDirty |= viewModel.GetIsDirty(question);

            viewModel.IsDirty |= viewModel.Categories.Count != question.QuestionCategories.Count;
            IList<QuestionCategory> sortedQuestionCategories = question.QuestionCategories.OrderBy(x => x.Category.ID).ToArray();
            IList<QuestionCategoryViewModel> sortedQuestionCategoryViewModels = viewModel.Categories.OrderBy(x => x.Category.ID).ToArray();
            for (int i = 0; i < viewModel.Categories.Count; i++)
            {
                viewModel.IsDirty |= sortedQuestionCategoryViewModels[i].GetIsDirty(sortedQuestionCategories[i]);
            }

            viewModel.IsDirty &= viewModel.Links.Count != question.QuestionLinks.Count;
            IList<QuestionLink> sortedQuestionLinks = question.QuestionLinks.OrderBy(x => x.ID).ToArray();
            IList<QuestionLinkViewModel> sortedQuestionLinkViewModels = viewModel.Links.OrderBy(x => x.ID).ToArray();
            for (int i = 0; i < viewModel.Links.Count; i++)
            {
                viewModel.IsDirty |= sortedQuestionLinkViewModels[i].GetIsDirty(sortedQuestionLinks[i]);
            }

            viewModel.IsDirty |= viewModel.Flags.Count != question.QuestionFlags.Count;
            IList<QuestionFlag> sortedQuestionFlags = question.QuestionFlags.OrderBy(x => x.ID).ToArray();
            IList<QuestionFlagViewModel> sortedQuestionFlagViewModels = viewModel.Flags.OrderBy(x => x.ID).ToArray();
            for (int i = 0; i < viewModel.Flags.Count; i++)
            {
                viewModel.IsDirty |= sortedQuestionFlagViewModels[i].SetIsDirty(sortedQuestionFlags[i]);
            }

            return viewModel.IsDirty;
        }

        public static bool SetIsDirty(this QuestionFlagViewModel viewModel, QuestionFlag entity)
        {
            return viewModel.IsDirty = IsDirty(viewModel, entity);
        }
    }
}
