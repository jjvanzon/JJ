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
        /// <summary>
        /// Converts the entity to a view model, but does not convert the related entities.
        /// (objects and lists will be created, though: no nulls.)
        /// </summary>
        public static QuestionViewModel ToViewModel(this Question entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            return new QuestionViewModel
            {
                ID = entity.ID,
                Text = entity.Text,
                IsActive = entity.IsActive,
                Answer = entity.Answers[0].Text, // TODO: Refactor to support multiple answers.
                Source = new SourceViewModel(),
                Type = new QuestionTypeViewModel(),
                Categories = new List<QuestionCategoryViewModel>(),
                Links = new List<QuestionLinkViewModel>(),
                Flags = new List<QuestionFlagViewModel>()
            };
        }

        public static QuestionDetailsViewModel ToDetailsViewModel(this Question question)
        {
            if (question == null) throw new ArgumentNullException("question");

            var viewModel = new QuestionDetailsViewModel();
            viewModel.Question = question.ToViewModel();
            viewModel.Question.Source = question.Source.ToViewModel();
            viewModel.Question.Type = question.QuestionType.ToViewModel();

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

            viewModel.Question.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();

            return viewModel;
        }

        public static QuestionEditViewModel ToEditViewModel(this Question question, ICategoryRepository categoryRepository, IFlagStatusRepository flagStatusRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");

            var viewModel = new QuestionEditViewModel
            {
                Question = question.ToViewModel(),
                FlagStatuses = ViewModelHelper.CreateFlagStatusListViewModel(flagStatusRepository),
                Categories = ViewModelHelper.CreateCategoryListViewModelRecursive(categoryRepository),
                ValidationMessages = new List<ValidationMessage>(),
                IsNew = false,
                CanDelete = true
            };

            viewModel.Question.Source = question.Source.ToViewModel();
            viewModel.Question.Type = question.QuestionType.ToViewModel();

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

        public static QuestionConfirmDeleteViewModel ToConfirmDeleteViewModel(this Question question)
        {
            if (question == null) throw new ArgumentNullException("question");

            return new QuestionConfirmDeleteViewModel
            {
                ID = question.ID,
                Question = question.Text
            };
        }
    }
}
