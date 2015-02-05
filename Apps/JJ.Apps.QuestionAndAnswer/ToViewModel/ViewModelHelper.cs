using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer;
using JJ.Models.Canonical;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Framework.Reflection;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using System.Globalization;
using JJ.Framework.PlatformCompatibility;
using JJ.Apps.QuestionAndAnswer.Helpers;
using System.Threading;
using JJ.Framework.Presentation;

namespace JJ.Apps.QuestionAndAnswer.ToViewModel
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

        public static PagingViewModel CreatePagingViewModel(int selectedPageIndex, int pageSize, int count, int maxVisiblePageNumbers)
        {
            if (pageSize < 1) throw new Exception("pageSize cannot be less than 1.");
            if (selectedPageIndex < 0) throw new Exception("selectedPageIndex cannot be less than 0");
            if (count < 0) throw new Exception("selectedPageIndex cannot be less than 0");
            if (maxVisiblePageNumbers < 1) throw new Exception("maxVisiblePageNumbers cannot be less than 1");

            int pageCount = (int)Math.Ceiling((decimal)count / (decimal)pageSize);
            if (selectedPageIndex > pageCount)
            {
                throw new Exception(String.Format("pageIndex {0} is larger than pageCount {1}.", selectedPageIndex, pageCount));
            }

            bool hasPages = pageCount != 0;
            bool isFirstPage = selectedPageIndex == 0;
            bool isLastPage = selectedPageIndex == pageCount - 1;

            var viewModel = new PagingViewModel
            {
                PageCount = pageCount,
                CanGoToPreviousPage = hasPages && !isFirstPage,
                CanGoToNextPage = hasPages && !isLastPage,
            };

            viewModel.CanGoToFirstPage = viewModel.CanGoToPreviousPage;
            viewModel.CanGoToLastPage = viewModel.CanGoToNextPage;

            // Get a max set of heading or trailing page numbers around the selected page number.
            int headingPageNumberCount = (maxVisiblePageNumbers - 1) / 2;

            int firstpageIndex = selectedPageIndex - headingPageNumberCount;
            if (firstpageIndex < 0) firstpageIndex = 0;

            int lastPageIndex = firstpageIndex + maxVisiblePageNumbers - 1;
            if (lastPageIndex > pageCount - 1) lastPageIndex = pageCount - 1;

            // Create page number view models
            viewModel.VisiblePageNumbers = new List<int>(maxVisiblePageNumbers);
            for (int i = firstpageIndex; i <= lastPageIndex; i++)
			{
                int pageNumber = i + 1;
                viewModel.VisiblePageNumbers.Add(pageNumber);
			}

            viewModel.PageNumber = selectedPageIndex + 1;

            viewModel.MustShowLeftEllipsis = firstpageIndex != 0;
            viewModel.MustShowRightEllipsis = lastPageIndex != pageCount;

            return viewModel;
        }
    }
}
