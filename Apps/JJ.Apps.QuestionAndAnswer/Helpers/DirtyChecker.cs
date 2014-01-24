using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Helpers
{
    public class DirtyChecker
    {
        public static bool IsDirty(QuestionViewModel viewModel, Question question)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            // If question is new it is always dirty.
            if (question == null) return true;

            viewModel.NullCoallesce();

            if (viewModel.Answer != question.Answers[0].Text) return true;
            if (viewModel.IsActive != question.IsActive) return true;
            if (viewModel.Text != question.Text) return true;

            if (viewModel.Type.ID != question.QuestionType.ID) return true;
            if (viewModel.Source.ID != question.Source.ID) return true;

            if (viewModel.Categories.Count != question.QuestionCategories.Count) return true;
            IList<Category> sortedCategories = question.QuestionCategories.Select(x => x.Category).OrderBy(x => x.ID).ToArray();
            IList<CategoryViewModel> sortedCategoryViewModels = viewModel.Categories.Select(x => x.Category).OrderBy(x => x.ID).ToArray();
            for (int i = 0; i < viewModel.Categories.Count; i++)
            {
                Category category = sortedCategories[i];
                CategoryViewModel categoryViewModel = sortedCategoryViewModels[i];
                if (category.ID != categoryViewModel.ID) return true;
            }

            if (viewModel.Links.Count != question.QuestionLinks.Count) return true;
            IList<QuestionLink> sortedQuestionLinks = question.QuestionLinks.OrderBy(x => x.ID).ToArray();
            IList<QuestionLinkViewModel> sortedQuestionLinkViewModels = viewModel.Links.OrderBy(x => x.ID).ToArray();
            for (int i = 0; i < viewModel.Links.Count; i++)
            {
                QuestionLink questionLink = sortedQuestionLinks[i];
                QuestionLinkViewModel questionLinkViewModel = sortedQuestionLinkViewModels[i];
                if (questionLink.Url != questionLinkViewModel.Url) return true;
                if (questionLink.Description != questionLinkViewModel.Description) return true;
            }

            if (viewModel.Flags.Count != question.QuestionFlags.Count) return true;
            IList<QuestionFlag> sortedQuestionFlags = question.QuestionFlags.OrderBy(x => x.ID).ToArray();
            IList<QuestionFlagViewModel> sortedQuestionFlagViewModels = viewModel.Flags.OrderBy(x => x.ID).ToArray();
            for (int i = 0; i < viewModel.Flags.Count; i++)
            {
                QuestionFlag questionFlag = sortedQuestionFlags[i];
                QuestionFlagViewModel questionFlagViewModel = sortedQuestionFlagViewModels[i];
                if (IsDirty(questionFlagViewModel, questionFlag)) return true;
                
            }

            return false;
        }

        public static bool IsDirty(QuestionFlagViewModel viewModel, QuestionFlag entity)
        {
            // If entity is new it is always dirty.
            if (entity == null) return true;

            if (entity.ID != viewModel.ID) return true;

            return false;
        }
    }
}
