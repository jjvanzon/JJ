using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class QuestionDetailViewModelExtensions
    {
        /// <summary>
        /// Fills up the viewmodel with new objects where there are unexpected nulls.
        /// </summary>
        public static QuestionDetailViewModel NullCoallesce(this QuestionDetailViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            viewModel.FlagStatuses = viewModel.FlagStatuses ?? new List<FlagStatusViewModel>();
            viewModel.Categories = viewModel.Categories ?? new List<CategoryViewModel>();
            viewModel.ValidationMessages = viewModel.ValidationMessages ?? new List<Models.Canonical.ValidationMessage>();

            viewModel.Question = viewModel.Question ?? new QuestionViewModel();
            viewModel.Question.Categories = viewModel.Question.Categories ?? new List<QuestionCategoryViewModel>();
            viewModel.Question.Links = viewModel.Question.Links ?? new List<QuestionLinkViewModel>();
            viewModel.Question.Flags = viewModel.Question.Flags ?? new List<QuestionFlagViewModel>();

            foreach (QuestionCategoryViewModel questionCategoryViewModel in viewModel.Question.Categories)
            {
                questionCategoryViewModel.Category = questionCategoryViewModel.Category ?? new CategoryViewModel();
                questionCategoryViewModel.Category.NameParts = questionCategoryViewModel.Category.NameParts ?? new List<string>();
            }

            return viewModel;
        }

        /// <summary>
        /// Converts a partially filled view model to an entity.
        /// First, a possibly existing entity is retrieved from the database.
        /// Next, the editable parts of the entity are taken over from the view model.
        /// </summary>
        public static Question ToEntity(
            this QuestionDetailViewModel viewModel, 
            IQuestionRepository questionRepository, 
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionCategoryRepository == null) throw new ArgumentNullException("questionCategoryRepository");
            if (questionLinkRepository == null) throw new ArgumentNullException("questionLinkRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");

            viewModel.NullCoallesce();

            // Question
            Question question = questionRepository.TryGet(viewModel.Question.ID);
            if (question == null)
            {
                question = questionRepository.Create();
                // TODO: Make sure you do this.
                //question.SetQuestionTypeEnum(questionTypeRepository, QuestionTypeEnum.OpenQuestion);
                question.AutoCreateRelatedEntities(answerRepository);
            }
            question.Text = viewModel.Question.Text;
            question.IsActive = viewModel.Question.IsActive;

            // Answer
            // TODO: Refactor to support multiple answers
            if (question.Answers.Count == 0)
            {
                question.Answers.Add(new Answer());
            }
            question.Answers[0].Text = viewModel.Question.Answer;

            // Categories

            // Add or update question categories
            foreach (QuestionCategoryViewModel questionCategoryViewModel in viewModel.Question.Categories)
            {
                QuestionCategory questionCategory = TryGetExistingQuestionCategory(questionCategoryViewModel, questionCategoryRepository);
                if (questionCategory == null)
                {
                    questionCategory = questionCategoryRepository.Create();
                    questionCategory.LinkTo(question);
                }

                // Newly added items might not have a category filled in yet.
                Category category = categoryRepository.TryGet(questionCategoryViewModel.Category.ID); // Empty view model question categories have Category.ID 0.
                questionCategory.LinkTo(category);
            }

            // Delete question categories
            IList<int> entityQuestionCategoryIDs = question.QuestionCategories.Where(x => x.ID != 0) // Exclude new entities which have ID 0.
                                                                              .Select(x => x.ID)
                                                                              .ToArray();

            IList<int> viewModelQuestionCategoryIDs = viewModel.Question.Categories.Where(x => x.QuestionCategoryID != 0) // Exclude new entities which have ID 0.
                                                                                   .Select(x => x.QuestionCategoryID)
                                                                                   .ToArray();

            IList<int> questionCategoryIDsToRemove = entityQuestionCategoryIDs.Except(viewModelQuestionCategoryIDs).ToArray();

            foreach (QuestionCategory questionCategory in question.QuestionCategories.ToArray())
            {
                if (questionCategoryIDsToRemove.Contains(questionCategory.ID))
                {
                    questionCategory.UnlinkRelatedEntities();
                    questionCategoryRepository.Delete(questionCategory);
                }
            }

            // Links

            // Add or update links
            foreach (QuestionLinkViewModel questionLinkViewModel in viewModel.Question.Links)
            {
                QuestionLink questionLink = TryGetExistingQuestionLink(questionLinkViewModel, questionLinkRepository);
                if (questionLink == null)
                {
                    questionLink = questionLinkRepository.Create();
                    questionLink.LinkTo(question);
                }

                questionLink.Url = questionLinkViewModel.Url;
                questionLink.Description = questionLinkViewModel.Description;
            }

            // Delete links
            IList<int> entityQuestionLinkIDs = question.QuestionLinks.Where(x => x.ID != 0) // Exclude new entities which have ID 0.
                                                                     .Select(x => x.ID)
                                                                     .ToArray();

            IList<int> viewModelQuestionLinkIDs = viewModel.Question.Links.Where(x => x.ID != 0) // Exclude new entities which have ID 0.
                                                                          .Select(x => x.ID)
                                                                          .ToArray();

            IList<int> questionLinkIDsToRemove = entityQuestionLinkIDs.Except(viewModelQuestionLinkIDs).ToArray();

            foreach (QuestionLink questionLink in question.QuestionLinks.ToArray())
            {
                if (questionLinkIDsToRemove.Contains(questionLink.ID))
                {
                    questionLink.UnlinkRelatedEntities();
                    questionLinkRepository.Delete(questionLink);
                }
            }

            // Update content flags
            foreach (QuestionFlagViewModel questionFlagViewModel in viewModel.Question.Flags)
            {
                QuestionFlag questionFlag = questionFlagRepository.TryGet(questionFlagViewModel.ID);
                if (questionFlag != null) // Concurrency
                {
                    questionFlag.FlagStatus = flagStatusRepository.Get(questionFlagViewModel.Status.ID);
                    questionFlag.Comment = questionFlagViewModel.Comment;
                }
            }

            return question;
        }

        private static QuestionLink TryGetExistingQuestionLink(QuestionLinkViewModel viewModel, IQuestionLinkRepository repository)
        {
            // Be careful adjusting this code. TryGet may return an existing new entity with ID 0 and may crash if there are multiple entities with ID 0.
            bool isNew = viewModel.ID == 0;
            if (!isNew)
            {
                return repository.TryGet(viewModel.ID); // TryGet will return null in case of ghost reads in high concurrency situations.
            }
            return null;
        }

        private static QuestionCategory TryGetExistingQuestionCategory(QuestionCategoryViewModel viewModel, IQuestionCategoryRepository repository)
        {
            // Be careful adjusting this code. TryGet may return an existing new entity with ID 0 and may crash if there are multiple entities with ID 0.
            bool isNew = viewModel.QuestionCategoryID == 0;
            if (!isNew)
            {
                return repository.TryGet(viewModel.QuestionCategoryID); // TryGet will return null in case of ghost reads in high concurrency situations.
            }
            return null;
        }
    }
}
