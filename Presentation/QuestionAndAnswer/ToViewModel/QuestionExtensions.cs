using System.Collections.Generic;
using System.Linq;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Data.Canonical;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;

namespace JJ.Presentation.QuestionAndAnswer.ToViewModel
{
    internal static class QuestionExtensions
    {
        public static QuestionDetailsViewModel ToDetailsViewModel(
            this Question question,
            IUserRepository userRepository,
            string authenticatedUserName)
        {
            var viewModel = new QuestionDetailsViewModel
            {
                Question = question.ToViewModel()
            };
            viewModel.Question.Source = question.Source.ToViewModel();
            viewModel.Question.Type = question.QuestionType.ToViewModel();

            // Partials
            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(authenticatedUserName, userRepository);

            // Categories
            foreach (QuestionCategory questionCategory in question.QuestionCategories)
            {
                QuestionCategoryViewModel questionCategoryViewModel = questionCategory.ToViewModel();
                viewModel.Question.Categories.Add(questionCategoryViewModel);
            }

            // Links
            foreach (QuestionLink questionLink in question.QuestionLinks)
            {
                QuestionLinkViewModel linkModel = questionLink.ToViewModel();
                viewModel.Question.Links.Add(linkModel);
            }

            // Flags
            foreach (QuestionFlag flag in question.QuestionFlags)
            {
                QuestionFlagViewModel flagViewModel = flag.ToViewModel();
                viewModel.Question.Flags.Add(flagViewModel);
            }

            viewModel.Question.IsFlagged =
                question.QuestionFlags.Where(x => x.GetFlagStatusEnum() == FlagStatusEnum.Flagged).Any();

            return viewModel;
        }

        public static QuestionEditViewModel ToEditViewModel(
            this Question question,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            string authenticatedUserName)
        {
            var viewModel = new QuestionEditViewModel
            {
                Question = question.ToViewModel(),
                ValidationMessages = new List<string>(),
                CanDelete = true,
                AllCategories = ViewModelHelper.CreateCategoryListViewModelRecursive(categoryRepository)
            };

            viewModel.Question.Source = question.Source.ToViewModel();
            viewModel.Question.Type = question.QuestionType.ToViewModel();

            // Partials
            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(authenticatedUserName, userRepository);

            // Links
            foreach (QuestionLink questionLink in question.QuestionLinks)
            {
                QuestionLinkViewModel linkModel = questionLink.ToViewModel();
                viewModel.Question.Links.Add(linkModel);
            }

            // Question categories
            foreach (QuestionCategory questionCategory in question.QuestionCategories)
            {
                QuestionCategoryViewModel questionCategoryViewModel = questionCategory.ToViewModel();
                viewModel.Question.Categories.Add(questionCategoryViewModel);
            }

            // Flags
            IList<IDAndName> allFlagStatuses = ViewModelHelper.CreateFlagStatusListViewModel();

            foreach (QuestionFlag flag in question.QuestionFlags)
            {
                QuestionFlagViewModel flagViewModel = flag.ToViewModel();
                flagViewModel.AllFlagStatuses = allFlagStatuses;
                viewModel.Question.Flags.Add(flagViewModel);
            }

            viewModel.Question.IsFlagged =
                question.QuestionFlags.Where(x => x.GetFlagStatusEnum() == FlagStatusEnum.Flagged).Any();

            return viewModel;
        }

        public static RandomQuestionViewModel ToRandomQuestionViewModel(
            this Question entity,
            IUserRepository userRepository,
            string authenticatedUserName,
            QuestionFlag currentUserQuestionFlag = null)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var viewModel = new RandomQuestionViewModel
            {
                SelectedCategories = new List<CategoryViewModel>(),
                Question = entity.ToViewModel()
            };

            // Partials
            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(authenticatedUserName, userRepository);
            viewModel.LanguageSelector = ViewModelHelper.CreateLanguageSelectionViewModel();

            // Links
            foreach (QuestionLink questionLink in entity.QuestionLinks)
            {
                QuestionLinkViewModel linkModel = questionLink.ToViewModel();
                viewModel.Question.Links.Add(linkModel);
            }

            // Categories
            foreach (QuestionCategory questionCategory in entity.QuestionCategories)
            {
                QuestionCategoryViewModel questionCategoryModel = questionCategory.ToViewModel();
                viewModel.Question.Categories.Add(questionCategoryModel);
            }

            // Current user flag
            if (currentUserQuestionFlag != null)
            {
                viewModel.CurrentUserQuestionFlag = currentUserQuestionFlag.ToCurrentUserQuestionFlagViewModel();
                viewModel.Question.IsFlagged = currentUserQuestionFlag.GetFlagStatusEnum() == FlagStatusEnum.Flagged;
            }
            else
            {
                viewModel.CurrentUserQuestionFlag = new CurrentUserQuestionFlagPartialViewModel();
            }

            return viewModel;
        }

        public static QuestionConfirmDeleteViewModel ToConfirmDeleteViewModel(
            this Question question,
            IUserRepository userRepository,
            string authenticatedUserName)
        {
            var viewModel = new QuestionConfirmDeleteViewModel
            {
                ID = question.ID,
                Question = question.Text,
                Login = ViewModelHelper.CreateLoginPartialViewModel(authenticatedUserName, userRepository)
            };

            return viewModel;
        }
    }
}