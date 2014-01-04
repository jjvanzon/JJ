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
                Categories = new List<CategoryViewModel>(),
                Flags = new List<QuestionFlagViewModel>()
            };
        }

        public static QuestionDetailViewModel ToDetailViewModel(this Question question, IFlagStatusRepository flagStatusRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");

            var viewModel = new QuestionDetailViewModel
            {
                Question = question.ToViewModel(),
                FlagStatuses = new List<FlagStatusViewModel>(),
                ValidationMessages = new List<ValidationMessage>()
            };

            // Links
            foreach (QuestionLink questionLink in question.QuestionLinks)
            {
                QuestionLinkViewModel linkModel = questionLink.ToViewModel();
                viewModel.Question.Links.Add(linkModel);
            }

            // Categories
            foreach (Category category in question.QuestionCategories.Select(x => x.Category))
            {
                CategoryViewModel categoryModel = category.ToViewModel();
                viewModel.Question.Categories.Add(categoryModel);
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
            foreach (Category category in entity.QuestionCategories.Select(x => x.Category))
            {
                CategoryViewModel categoryModel = category.ToViewModel();
                viewModel.Question.Categories.Add(categoryModel);
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

        /*private static QuestionViewModel ToViewModelWithRelatedEntities(this Question entity, QuestionFlag currentUserQuestionFlag = null)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            QuestionViewModel viewModel = entity.ToViewModel();

            // Links
            foreach (QuestionLink questionLink in entity.QuestionLinks)
            {
                var linkModel = new LinkViewModel(questionLink.Description, questionLink.Url);
                viewModel.Links.Add(linkModel);
            }

            // Categories
            foreach (Category category in entity.QuestionCategories.Select(x => x.Category))
            {
                CategoryViewModel categoryModel = category.ToViewModel();
                viewModel.Categories.Add(categoryModel);
            }

            // Flags
            foreach (QuestionFlag flag in entity.QuestionFlags)
            {
                QuestionFlagViewModel flagViewModel = flag.ToViewModel();
                viewModel.Flags.Add(flagViewModel);
            }

            // Current user flag
            if (currentUserQuestionFlag != null)
            {
                viewModel.CurrentUserFlag = currentUserQuestionFlag.ToViewModel();
            }

            return viewModel;
        }*/
    }
}
