using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Models.Canonical;
using JJ.Business.QuestionAndAnswer;
using JJ.Framework.Common;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class QuestionExtensions
    {
        public static QuestionViewModel ToViewModel(this Question entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            return new QuestionViewModel
            {
                ID = entity.ID,
                Text = entity.Text,
                IsActive = entity.IsActive,
                Answer = entity.Answers[0].Text, // TODO: Refactor
                Links = new List<QuestionLinkViewModel>(),
                Categories = new List<QuestionCategoryViewModel>(),
                Flags = new List<QuestionFlagViewModel>()
            };
        }

        public static QuestionDetailViewModel ToDetailViewModel(this Question question, IFlagStatusRepository flagStatusRepository, ICategoryRepository categoryRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");

            var viewModel = new QuestionDetailViewModel
            {
                Question = question.ToViewModel(),
                FlagStatuses = new List<FlagStatusViewModel>(),
                Categories = GetCategoriesViewModelRecursive(categoryRepository),
                ValidationMessages = new List<ValidationMessage>()
            };

            // Links
            foreach (QuestionLink questionLink in question.QuestionLinks)
            {
                QuestionLinkViewModel linkModel = questionLink.ToViewModel();
                viewModel.Question.Links.Add(linkModel);
            }

            // Categories
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

            // Flag statuses
            foreach (FlagStatus flagStatus in flagStatusRepository.GetAll().ToArray())
            {
                FlagStatusViewModel flagStatusViewModel = flagStatus.ToViewModel();
                viewModel.FlagStatuses.Add(flagStatusViewModel);
            }

            viewModel.Question.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();

            return viewModel;
        }

        /// <summary> Gets a tree of category view models. </summary>
        private static IList<CategoryViewModel> GetCategoriesViewModelRecursive(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) { throw new ArgumentNullException("categoryRepository"); }

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

        public static RandomQuestionViewModel ToRandomQuestionViewModel(this Question entity, QuestionFlag currentUserQuestionFlag = null)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            var viewModel = new RandomQuestionViewModel()
            {
                SelectedCategories = new List<CategoryViewModel>(),
                Question = entity.ToViewModel()
            };

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
                viewModel.CurrentUserQuestionFlag = new CurrentUserQuestionFlagViewModel();
            }

            return viewModel;
        }
    }
}
