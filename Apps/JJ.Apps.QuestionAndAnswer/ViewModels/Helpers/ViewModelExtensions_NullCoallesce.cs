using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class ViewModelExtensions_NullCoallesce
    {
        // TODO: Consider null-coallescing the collection items.

        /// <summary>
        /// Fills up the viewmodel with new objects where there are unexpected nulls.
        /// </summary>
        public static void NullCoallesce(this QuestionEditViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.FlagStatuses = viewModel.FlagStatuses ?? new List<FlagStatusViewModel>();
            viewModel.Categories = viewModel.Categories ?? new List<CategoryViewModel>();
            viewModel.ValidationMessages = viewModel.ValidationMessages ?? new List<Models.Canonical.ValidationMessage>();
            viewModel.Question = viewModel.Question ?? new QuestionViewModel();

            viewModel.Question.NullCoallesce();
        }

        /// <summary>
        /// Fills up the viewmodel with new objects where there are unexpected nulls.
        /// </summary>
        public static void NullCoallesce(this QuestionViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.Source = viewModel.Source ?? new SourceViewModel();
            viewModel.Type = viewModel.Type ?? new QuestionTypeViewModel();
            viewModel.Categories = viewModel.Categories ?? new ListViewModel<QuestionCategoryViewModel>();
            viewModel.Links = viewModel.Links ?? new ListViewModel<QuestionLinkViewModel>();
            viewModel.Flags = viewModel.Flags ?? new ListViewModel<QuestionFlagViewModel>();

            foreach (QuestionCategoryViewModel questionCategoryViewModel in viewModel.Categories)
            {
                NullCoallesce(questionCategoryViewModel);
            }

            foreach (QuestionLinkViewModel questionLinkViewModel in viewModel.Links)
            {
                NullCoallesce(questionLinkViewModel);
            }

            foreach (QuestionFlagViewModel questionFlagViewModel in viewModel.Flags)
            {
                NullCoallesce(questionFlagViewModel);
            }
        }

        /// <summary>
        /// Fills up the viewmodel with new objects where there are unexpected nulls.
        /// </summary>
        private static void NullCoallesce(QuestionLinkViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            // No child-objects to null-coallesce.
        }

        /// <summary>
        /// Fills up the viewmodel with new objects where there are unexpected nulls.
        /// </summary>
        public static void NullCoallesce(this QuestionCategoryViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.Category = viewModel.Category ?? new CategoryViewModel();
            viewModel.Category.NameParts = viewModel.Category.NameParts ?? new List<string>();
        }

        /// <summary>
        /// Fills up the viewmodel with new objects where there are unexpected nulls.
        /// </summary>
        public static void NullCoallesce(this QuestionFlagViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.Status = viewModel.Status ?? new FlagStatusViewModel();
        }
    }
}
