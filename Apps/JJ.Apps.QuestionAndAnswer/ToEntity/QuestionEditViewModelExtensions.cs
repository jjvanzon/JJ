using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Reflection;

namespace JJ.Apps.QuestionAndAnswer.ToEntity
{
    internal static class QuestionEditViewModelExtensions
    {
        /// <summary>
        /// Converts a partially filled view model to an entity.
        /// First, a possibly existing entity is retrieved from the database.
        /// Next, the editable parts of the entity are taken over from the view model.
        /// </summary>
        public static Question ToEntity(
            this QuestionEditViewModel viewModel, 
            Repositories repositories)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (repositories == null) throw new NullException(() => repositories);

            viewModel.NullCoallesce();

            // Question
            Question question = repositories.QuestionRepository.TryGet(viewModel.Question.ID);
            if (question == null)
            {
                question = repositories.QuestionRepository.Create();
                question.AutoCreateRelatedEntities(repositories.AnswerRepository);
            }
            question.Text = viewModel.Question.Text;
            question.IsActive = viewModel.Question.IsActive;
            question.Source = repositories.SourceRepository.Get(viewModel.Question.Source.ID);
            question.QuestionType = repositories.QuestionTypeRepository.Get(viewModel.Question.Type.ID);

            // Answer
            // TODO: Refactor to support multiple answers
            if (question.Answers.Count == 0)
            {
                question.Answers.Add(new Answer());
            }
            question.Answers[0].IsCorrectAnswer = true;
            question.Answers[0].Text = viewModel.Question.Answer;

            // Categories

            // Add or update question categories
            foreach (QuestionCategoryViewModel questionCategoryViewModel in viewModel.Question.Categories)
            {
                QuestionCategory questionCategory = TryGetExistingQuestionCategory(questionCategoryViewModel, repositories.QuestionCategoryRepository);
                if (questionCategory == null)
                {
                    questionCategory = repositories.QuestionCategoryRepository.Create();
                    questionCategory.LinkTo(question);
                }

                // Newly added items might not have a category filled in yet.
                Category category = repositories.CategoryRepository.TryGet(questionCategoryViewModel.Category.ID); // Empty view model question categories have Category.ID 0.
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
                    repositories.QuestionCategoryRepository.Delete(questionCategory);
                }
            }

            // Links

            // Add or update links
            foreach (QuestionLinkViewModel questionLinkViewModel in viewModel.Question.Links)
            {
                QuestionLink questionLink = TryGetExistingQuestionLink(questionLinkViewModel, repositories.QuestionLinkRepository);
                if (questionLink == null)
                {
                    questionLink = repositories.QuestionLinkRepository.Create();
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
                    repositories.QuestionLinkRepository.Delete(questionLink);
                }
            }

            // Update content flags
            foreach (QuestionFlagViewModel questionFlagViewModel in viewModel.Question.Flags)
            {
                QuestionFlag questionFlag = repositories.QuestionFlagRepository.TryGet(questionFlagViewModel.ID);
                if (questionFlag != null) // Concurrency
                {
                    questionFlag.FlagStatus = repositories.FlagStatusRepository.Get(questionFlagViewModel.Status.ID);
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
