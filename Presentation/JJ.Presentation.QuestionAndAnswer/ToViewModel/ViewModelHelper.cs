using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer;
using JJ.Business.CanonicalModel;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Framework.Reflection;
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

        public static PagerViewModel CreatePagerViewModel(int selectedPageIndex, int pageSize, int count, int maxVisiblePageNumbers)
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

            var viewModel = new PagerViewModel
            {
                PageCount = pageCount,
                CanGoToPreviousPage = hasPages && !isFirstPage,
                CanGoToNextPage = hasPages && !isLastPage,
            };

            viewModel.CanGoToFirstPage = viewModel.CanGoToPreviousPage;
            viewModel.CanGoToLastPage = viewModel.CanGoToNextPage;

            // Get a max set of heading or trailing page numbers around the selected page number.

            // This did not work when we were at the end and not all page numbers fit. 
            // Then we would end up with half of the maxVisiblePageNumbers.
            //int numberOfPagesOnLeftSide = (maxVisiblePageNumbers - 1) / 2;

            //int firstVisiblePageIndex = selectedPageIndex - numberOfPagesOnLeftSide;
            //if (firstVisiblePageIndex < 0) firstVisiblePageIndex = 0;

            //int lastVisiblePageIndex = firstVisiblePageIndex + maxVisiblePageNumbers - 1;
            //if (lastVisiblePageIndex > pageCount - 1) lastVisiblePageIndex = pageCount - 1;

            // There must be a simpler way than this, but I cannot figure it out.
            int firstVisiblePageIndex;
            int lastVisiblePageIndex;

            bool allPageNumbersAreVisible = pageCount <= maxVisiblePageNumbers;
            if (allPageNumbersAreVisible)
            {
                firstVisiblePageIndex = 0;
                lastVisiblePageIndex = pageCount - 1;
            }
            else
            {
                // Numbers do not fit.

                // The -1 is the selected page.
                int numberOfPagesOnLeftSide = (maxVisiblePageNumbers - 1) / 2; // Sneekily make use of integer division to get less on the left side in case of an even number of visible pages.
                int numberOfPagesOnRightSide = maxVisiblePageNumbers - numberOfPagesOnLeftSide - 1;

                bool isLeftBound = selectedPageIndex - numberOfPagesOnLeftSide <= 0;
                bool isRightBound = selectedPageIndex + numberOfPagesOnRightSide > pageCount - 1;

                if (isLeftBound)
                {
                    firstVisiblePageIndex = 0;
                    lastVisiblePageIndex = maxVisiblePageNumbers - 1;
                }
                else if (isRightBound)
                {
                    lastVisiblePageIndex = pageCount - 1;
                    firstVisiblePageIndex = pageCount - maxVisiblePageNumbers;
                }
                else
                {
                    // Is is somewhere in the middle.
                    firstVisiblePageIndex = selectedPageIndex - numberOfPagesOnLeftSide;
                    lastVisiblePageIndex = selectedPageIndex + numberOfPagesOnRightSide;
                }
            }

            // Create page number view models
            viewModel.VisiblePageNumbers = new List<int>(maxVisiblePageNumbers);
            for (int i = firstVisiblePageIndex; i <= lastVisiblePageIndex; i++)
			{
                int pageNumber = i + 1;
                viewModel.VisiblePageNumbers.Add(pageNumber);
			}

            viewModel.PageNumber = selectedPageIndex + 1;

            viewModel.MustShowLeftEllipsis = firstVisiblePageIndex != 0;
            viewModel.MustShowRightEllipsis = lastVisiblePageIndex != pageCount - 1;

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
