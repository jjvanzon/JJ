using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Extensions
{
    internal static class ViewModelExtensions_NullCoalesce
    {
        public static void NullCoalesce(this QuestionEditViewModel viewModel)
        {
            viewModel.Login = viewModel.Login ?? new LoginPartialViewModel();

            viewModel.FlagStatuses = viewModel.FlagStatuses ?? new List<FlagStatusViewModel>();
            viewModel.Categories = viewModel.Categories ?? new List<CategoryViewModel>();
            viewModel.ValidationMessages = viewModel.ValidationMessages ?? new List<Models.Canonical.ValidationMessage>();
            viewModel.Question = viewModel.Question ?? new QuestionViewModel();

            viewModel.Question.NullCoalesce();
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
            viewModel.Status = viewModel.Status ?? new FlagStatusViewModel();
        }

        public static void NullCoalesce(this CategorySelectorViewModel viewModel)
        {
            viewModel.Login = viewModel.Login ?? new LoginPartialViewModel();
            viewModel.LanguageSelector = viewModel.LanguageSelector ?? new LanguageSelectorPartialViewModel();
            viewModel.LanguageSelector.NullCoalesce();

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
