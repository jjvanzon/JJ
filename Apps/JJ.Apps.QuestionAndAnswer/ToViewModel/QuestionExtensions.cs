using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Models.Canonical;
using JJ.Business.QuestionAndAnswer;
using JJ.Framework.Common;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Framework.Reflection;

namespace JJ.Apps.QuestionAndAnswer.ToViewModel
{
    internal static class QuestionExtensions
    {
        public static QuestionDetailsViewModel ToDetailsViewModel(this Question question, IUserRepository userRepository, string authenticatedUserName)
        {
            var viewModel = new QuestionDetailsViewModel();
            viewModel.Question = question.ToViewModel();
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

            viewModel.Question.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();

            return viewModel;
        }

        public static QuestionEditViewModel ToEditViewModel(
            this Question question, 
            ICategoryRepository categoryRepository, 
            IFlagStatusRepository flagStatusRepository, 
            IUserRepository userRepository,
            string authenticatedUserName)
        {
            if (flagStatusRepository == null) throw new NullException(() => flagStatusRepository);

            var viewModel = new QuestionEditViewModel
            {
                Question = question.ToViewModel(),
                FlagStatuses = ViewModelHelper.CreateFlagStatusListViewModel(flagStatusRepository),
                Categories = ViewModelHelper.CreateCategoryListViewModelRecursive(categoryRepository),
                ValidationMessages = new List<ValidationMessage>(),
                CanDelete = true
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
            foreach (QuestionFlag flag in question.QuestionFlags)
            {
                QuestionFlagViewModel flagViewModel = flag.ToViewModel();
                viewModel.Question.Flags.Add(flagViewModel);
            }

            viewModel.Question.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();

            return viewModel;
        }

        public static RandomQuestionViewModel ToRandomQuestionViewModel(this Question entity, IUserRepository userRepository, string authenticatedUserName, QuestionFlag currentUserQuestionFlag = null)
        {
            var viewModel = new RandomQuestionViewModel()
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
                viewModel.Question.IsFlagged = currentUserQuestionFlag.FlagStatus.ID == (int)FlagStatusEnum.Flagged;
            }
            else
            {
                viewModel.CurrentUserQuestionFlag = new CurrentUserQuestionFlagPartialViewModel();
            }

            return viewModel;
        }

        public static QuestionConfirmDeleteViewModel ToConfirmDeleteViewModel(this Question question, IUserRepository userRepository, string authenticatedUserName)
        {
            var viewModel = new QuestionConfirmDeleteViewModel
            {
                ID = question.ID,
                Question = question.Text
            };

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(authenticatedUserName, userRepository);

            return viewModel;
        }

        public static QuestionDeleteConfirmedViewModel ToDeleteConfirmedViewModel(this Question question, IUserRepository userRepository, string authenticatedUserName)
        {
            var viewModel = new QuestionDeleteConfirmedViewModel
            {
                ID = question.ID
            };

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(authenticatedUserName, userRepository);

            return viewModel;
        }
    }
}
