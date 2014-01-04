using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (viewModel.Question == null) throw new ArgumentNullException("viewModel.Question");
            if (viewModel.Question.Categories == null) throw new ArgumentNullException("viewModel.Question.Categories");
            if (viewModel.Question.Links == null) throw new ArgumentNullException("viewModel.Question.Links");
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionCategoryRepository == null) throw new ArgumentNullException("questionCategoryRepository");
            if (questionLinkRepository == null) throw new ArgumentNullException("questionLinkRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");

            // Question
            Question question = questionRepository.TryGet(viewModel.Question.ID);
            if (question == null)
            {
                question = questionRepository.Create();
                question.AutoCreateRelatedEntities(answerRepository);
            }
            question.Text = viewModel.Question.Text;
            question.IsActive = viewModel.Question.IsActive;

            // Answer
            // TODO: Refactor
            if (question.Answers.Count == 0)
            {
                question.Answers.Add(new Answer());
            }
            question.Answers[0].Text = viewModel.Question.Answer;

            // Categories
            IList<int> entityCategoryIDs = question.QuestionCategories.Select(x => x.Category.ID).ToArray();
            IList<int> viewModelCategoryIDs = viewModel.Question.Categories.Select(x => x.ID).ToArray();

            // Add question categories
            IList<int> categoryIDsToAdd = viewModelCategoryIDs.Except(entityCategoryIDs).ToArray();
            foreach (int categoryID in categoryIDsToAdd)
            {
                QuestionCategory questionCategory = questionCategoryRepository.Create();
                questionCategory.LinkTo(question);
                Category category = categoryRepository.Get(categoryID);
                questionCategory.LinkTo(category);
            }

            // Delete question categories
            IList<int> categoryIDsToRemove = entityCategoryIDs.Except(viewModelCategoryIDs).ToArray();
            IList<QuestionCategory> questionCategiesToDelete = question.QuestionCategories.Where(x => categoryIDsToRemove.Contains(x.ID)).ToArray();
            foreach (QuestionCategory questionCategory in questionCategiesToDelete)
            {
                questionCategory.UnlinkRelatedEntities();
                questionCategoryRepository.Delete(questionCategory);
            }

            // Links

            // Add or update links
            foreach (QuestionLinkViewModel questionLinkViewModel in viewModel.Question.Links)
            {
                QuestionLink questionLink = null;
                if (questionLinkViewModel.ID != 0) // New entities have ID 0 and don't exist in the database.
                {
                    questionLink = questionLinkRepository.TryGet(questionLinkViewModel.ID); // TryGet will return null for both new objects (with ID 0) and in case of ghost reads in high concurrency situations.
                }

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
    }
}
