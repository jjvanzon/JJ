using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System.Collections.Generic;
using JJ.Data.Canonical;

namespace JJ.Presentation.QuestionAndAnswer.Extensions
{
    internal static class ViewModelExtensions_NullCoalesce
    {
        public static void NullCoalesce(this QuestionEditViewModel viewModel)
        {
            viewModel.Login = viewModel.Login ?? new LoginPartialViewModel();

            viewModel.ValidationMessages = viewModel.ValidationMessages ?? new List<JJ.Data.Canonical.MessageDto>();
            viewModel.Question = viewModel.Question ?? new QuestionViewModel();
            viewModel.AllCategories = viewModel.AllCategories ?? new List<CategoryViewModel>();

            viewModel.Question.NullCoalesce();

            foreach (CategoryViewModel viewModel2 in viewModel.AllCategories)
            {
                viewModel2.NullCoalesce();
            }
        }

        public static void NullCoalesce(this QuestionViewModel viewModel)
        {
            viewModel.Source = viewModel.Source ?? new SourceViewModel();
            viewModel.Type = viewModel.Type ?? new QuestionTypeViewModel();
            viewModel.Categories = viewModel.Categories ?? new List<QuestionCategoryViewModel>();
            viewModel.Links = viewModel.Links ?? new List<QuestionLinkViewModel>();
            viewModel.Flags = viewModel.Flags ?? new List<QuestionFlagViewModel>();

            foreach (QuestionCategoryViewModel viewModel2 in viewModel.Categories)
            {
                viewModel2.NullCoalesce();
            }

            foreach (QuestionFlagViewModel viewModel2 in viewModel.Flags)
            {
                NullCoalesce(viewModel2);
            }
        }

        public static void NullCoalesce(this QuestionCategoryViewModel viewModel)
        {
            viewModel.Category = viewModel.Category ?? new CategoryViewModel();

            viewModel.Category.NullCoalesce();
        }

        public static void NullCoalesce(this QuestionFlagViewModel viewModel)
        {
            viewModel.Status = viewModel.Status ?? new IDAndName();

            viewModel.AllFlagStatuses = viewModel.AllFlagStatuses ?? new List<IDAndName>();
        }

        public static void NullCoalesce(this CategorySelectorViewModel viewModel)
        {
            viewModel.Login = viewModel.Login ?? new LoginPartialViewModel();

            viewModel.AvailableCategories = viewModel.AvailableCategories ?? new List<CategoryViewModel>();
            viewModel.SelectedCategories = viewModel.SelectedCategories ?? new List<CategoryViewModel>();

            foreach (CategoryViewModel viewModel2 in viewModel.AvailableCategories)
            {
                viewModel2.NullCoalesce();
            }

            foreach (CategoryViewModel viewModel2 in viewModel.SelectedCategories)
            {
                viewModel2.NullCoalesce();
            }
        }

        public static void NullCoalesce(this LanguageSelectorPartialViewModel viewModel)
        {
            viewModel.Languages = viewModel.Languages ?? new List<LanguageViewModel>();
        }

        public static void NullCoalesce(this CategoryViewModel viewModel)
        {
            viewModel.NameParts = viewModel.NameParts ?? new List<string>();
            viewModel.SubCategories = viewModel.SubCategories ?? new List<CategoryViewModel>();

            foreach (CategoryViewModel viewModel2 in viewModel.SubCategories)
            {
                viewModel2.NullCoalesce();
            }
        }

        public static void NullCoalesce(this QuestionConfirmDeleteViewModel viewModel)
        {
            viewModel.Login = viewModel.Login ?? new LoginPartialViewModel();
        }

        public static void NullCoalesce(this QuestionDeleteConfirmedViewModel viewModel)
        {
            viewModel.Login = viewModel.Login ?? new LoginPartialViewModel();
        }

        public static void NullCoalesce(this RandomQuestionViewModel viewModel)
        {
            viewModel.Login = viewModel.Login ?? new LoginPartialViewModel();
            viewModel.LanguageSelector = viewModel.LanguageSelector ?? new LanguageSelectorPartialViewModel();

            viewModel.LanguageSelector.NullCoalesce();

            viewModel.Question = viewModel.Question ?? new QuestionViewModel();
            viewModel.CurrentUserQuestionFlag = viewModel.CurrentUserQuestionFlag ?? new CurrentUserQuestionFlagPartialViewModel();
            viewModel.SelectedCategories = viewModel.SelectedCategories ?? new List<CategoryViewModel>();

            viewModel.Question.NullCoalesce();

            foreach (CategoryViewModel viewModel2 in viewModel.SelectedCategories)
            {
                viewModel2.NullCoalesce();
            }
        }
    }
}
