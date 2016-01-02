using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer;
using JJ.Data.Canonical;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System.Globalization;
using JJ.Framework.PlatformCompatibility;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using System.Threading;
using JJ.Framework.Presentation;

namespace JJ.Presentation.QuestionAndAnswer.ToViewModel
{
    internal static class ViewModelHelper
    {
        public static IList<FlagStatusViewModel> CreateFlagStatusListViewModel(IFlagStatusRepository flagStatusRepository)
        {
            if (flagStatusRepository == null) throw new NullException(() => flagStatusRepository);

            var list = new List<FlagStatusViewModel>();

            foreach (FlagStatus flagStatus in flagStatusRepository.GetAll().ToArray())
            {
                FlagStatusViewModel flagStatusViewModel = flagStatus.ToViewModel();
                list.Add(flagStatusViewModel);
            }

            return list;
        }

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
        {
            return new CategoryViewModel
            {
                NameParts = new List<string>(),
                SubCategories = new List<CategoryViewModel>(),
                Visible = true
            };
        }

        /// <param name="authenticatedUserName">nullable</param>
        public static LoginPartialViewModel CreateLoginPartialViewModel(string authenticatedUserName, IUserRepository userRepository)
        {
            if (userRepository == null) throw new NullException(() => userRepository);

            User user = userRepository.TryGetByUserName(authenticatedUserName);
            if (user != null)
            {
                return user.ToLoginPartialViewModel();
            }
            else
            {
                return ViewModelHelper.CreateLoggedOutLoginPartialViewModel();
            }
        }

        private static LoginPartialViewModel CreateLoggedOutLoginPartialViewModel()
        {
            return new LoginPartialViewModel
            {
                CanLogIn = true
            };
        }

        public static LanguageSelectorPartialViewModel CreateLanguageSelectionViewModel()
        {
            return ViewModelHelper.CreateLanguageSelectionViewModel(CultureHelper.GetAvailableCultureNames(), CultureHelper.GetCurrentCultureName());
        }

        public static LanguageSelectorPartialViewModel CreateLanguageSelectionViewModel(string cultureName)
        {
            return ViewModelHelper.CreateLanguageSelectionViewModel(CultureHelper.GetAvailableCultureNames(), cultureName);
        }

        public static LanguageSelectorPartialViewModel CreateLanguageSelectionViewModel(IList<string> availableCultureNames, string selectedCultureName)
        {
            var viewModel = new LanguageSelectorPartialViewModel();

            // Fill culture list
            viewModel.Languages = new List<LanguageViewModel>();

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

        public static QuestionDeleteConfirmedViewModel CreateDeleteConfirmedViewModel(int questionID, IUserRepository userRepository, string authenticatedUserName)
        {
            var viewModel = new QuestionDeleteConfirmedViewModel
            {
                ID = questionID
            };

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(authenticatedUserName, userRepository);

            return viewModel;
        }
    }
}
