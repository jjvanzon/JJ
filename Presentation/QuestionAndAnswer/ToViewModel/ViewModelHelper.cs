using System.Collections.Generic;
using System.Globalization;
using JJ.Business.Canonical;
using JJ.Business.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Data.Canonical;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.PlatformCompatibility;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;

namespace JJ.Presentation.QuestionAndAnswer.ToViewModel
{
    internal static class ViewModelHelper
    {
        public static IList<IDAndName> CreateFlagStatusListViewModel()
            => EnumToIDAndNameConverter.Convert<FlagStatusEnum>(ResourceFormatter.ResourceManager, mustIncludeUndefined: false);

        /// <summary> Gets a tree of category view models. </summary>
        public static IList<CategoryViewModel> CreateCategoryListViewModelRecursive(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) throw new NullException(() => categoryRepository);

            var categoryManager = new CategoryManager(categoryRepository);

            IEnumerable<Category> categories = categoryManager.GetCategoryTree();

            var viewModels = new List<CategoryViewModel>();

            foreach (Category category in categories)
            {
                CategoryViewModel viewModel = category.ToViewModelRecursive();
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static CategoryViewModel CreateEmptyCategoryViewModel()
            => new CategoryViewModel
            {
                NameParts = new List<string>(),
                SubCategories = new List<CategoryViewModel>(),
                Visible = true
            };

        /// <param name="authenticatedUserName">nullable</param>
        public static LoginPartialViewModel CreateLoginPartialViewModel(string authenticatedUserName, IUserRepository userRepository)
        {
            if (userRepository == null) throw new NullException(() => userRepository);

            User user = userRepository.TryGetByUserName(authenticatedUserName);
            if (user != null)
            {
                return user.ToLoginPartialViewModel();
            }

            return CreateLoggedOutLoginPartialViewModel();
        }

        private static LoginPartialViewModel CreateLoggedOutLoginPartialViewModel()
            => new LoginPartialViewModel
            {
                CanLogIn = true
            };

        public static LanguageSelectorPartialViewModel CreateLanguageSelectionViewModel()
            => CreateLanguageSelectionViewModel(CultureHelper.GetAvailableCultureNames(), CultureHelper.GetCurrentCultureName());

        public static LanguageSelectorPartialViewModel CreateLanguageSelectionViewModel(string cultureName)
            => CreateLanguageSelectionViewModel(CultureHelper.GetAvailableCultureNames(), cultureName);

        public static LanguageSelectorPartialViewModel CreateLanguageSelectionViewModel(
            IList<string> availableCultureNames,
            string selectedCultureName)
        {
            var viewModel = new LanguageSelectorPartialViewModel { Languages = new List<LanguageViewModel>() };

            // Fill culture list

            foreach (string cultureName in availableCultureNames)
            {
                CultureInfo cultureInfo = CultureInfo_PlatformSafe.GetCultureInfo(cultureName);

                var language = new LanguageViewModel
                {
                    CultureName = cultureName,
                    Name = cultureInfo.NativeName
                };

                viewModel.Languages.Add(language);
            }

            // Set selected culture
            string currentCultureName = CultureInfo.CurrentUICulture.Name;
            if (availableCultureNames.Contains(currentCultureName))
            {
                viewModel.SelectedLanguageCultureName = currentCultureName;
            }

            return viewModel;
        }

        public static LoginViewModel CreateLoginViewModel()
        {
            var viewModel = new LoginViewModel
            {
                LanguageSelector = CreateLanguageSelectionViewModel()
            };

            return viewModel;
        }

        public static QuestionDeleteConfirmedViewModel CreateDeleteConfirmedViewModel(
            int questionID,
            IUserRepository userRepository,
            string authenticatedUserName)
        {
            var viewModel = new QuestionDeleteConfirmedViewModel
            {
                ID = questionID
            };

            viewModel.Login = CreateLoginPartialViewModel(authenticatedUserName, userRepository);

            return viewModel;
        }
    }
}