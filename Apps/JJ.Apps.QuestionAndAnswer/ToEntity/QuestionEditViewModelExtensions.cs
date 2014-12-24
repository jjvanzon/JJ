using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;

namespace JJ.Apps.QuestionAndAnswer.ToEntity
{
    internal static class QuestionEditViewModelExtensions
    {
        public static Question ToEntity(this QuestionEditViewModel viewModel, Repositories repositories)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (repositories == null) throw new NullException(() => repositories);

            viewModel.NullCoallesce();

            // Question
            Question question = viewModel.Question.ToEntity(repositories.QuestionRepository, repositories.AnswerRepository, repositories.SourceRepository, repositories.QuestionTypeRepository);

            // Answer
            // For now multiple answers are not supported.
            Answer answer = viewModel.Question.ToAnswer(repositories.AnswerRepository);
            question.Answers[0] = answer;
            answer.Question = question;

            // Flags
            foreach (QuestionFlagViewModel questionFlagViewModel in viewModel.Question.Flags)
            {
                QuestionFlag questionFlag2 = questionFlagViewModel.ToEntity(repositories.QuestionFlagRepository, repositories.FlagStatusRepository);
                questionFlag2.LinkTo(question);
            }

            // Categories
            IList<QuestionCategory> newQuestionCategories = ConvertQuestionCategories(viewModel.Question.Categories, question.QuestionCategories, repositories.QuestionCategoryRepository, repositories.CategoryRepository);
            foreach (QuestionCategory newQuestionCategory in newQuestionCategories)
            {
                newQuestionCategory.LinkTo(question);
            }

            // Links
            IList<QuestionLink> newQuestionLinks = ConvertQuestionLinks(viewModel.Question.Links, question.QuestionLinks, repositories.QuestionLinkRepository);
            foreach (QuestionLink newQuestionLink in newQuestionLinks)
            {
                newQuestionLink.LinkTo(question);
            }

            return question;
        }

        /// <summary> Returns newly created entities. </summary>
        private static IList<QuestionCategory> ConvertQuestionCategories(
            IList<QuestionCategoryViewModel> viewModels,
            IList<QuestionCategory> entities,
            IQuestionCategoryRepository questionCategoryRepository, 
            ICategoryRepository categoryRepository)
        {
            var newEntities = new List<QuestionCategory>();

            foreach (QuestionCategoryViewModel viewModel in viewModels)
            {
                QuestionCategory entity = viewModel.ToEntity(questionCategoryRepository, categoryRepository);

                bool isNew = viewModel.QuestionCategoryID == 0;
                if (isNew)
                {
                    newEntities.Add(entity);
                }
            }

            // Delete
            ISet<int> entityIDs = new HashSet<int>(entities.Select(x => x.ID)
                                                           .Where(x => x != 0));

            ISet<int> viewModelIDs = new HashSet<int>(viewModels.Select(x => x.QuestionCategoryID)
                                                                .Where(x => x != 0));

            ISet<int> idsToDelete = new HashSet<int>(entityIDs.Except(viewModelIDs));

            foreach (QuestionCategory entity in entities.ToArray())
            {
                if (idsToDelete.Contains(entity.ID))
                {
                    entity.UnlinkRelatedEntities();
                    questionCategoryRepository.Delete(entity);
                }
            }

            return newEntities;
        }

        /// <summary> Returns newly created entities. </summary>
        private static IList<QuestionLink> ConvertQuestionLinks(
            IList<QuestionLinkViewModel> viewModels, 
            IList<QuestionLink> entities,
            IQuestionLinkRepository questionLinkRepository)
        {
            var newEntities = new List<QuestionLink>();

            foreach (QuestionLinkViewModel viewModel in viewModels)
            {
                QuestionLink entity = viewModel.ToEntity(questionLinkRepository);

                bool isNew = viewModel.ID == 0;
                if (isNew)
                {
                    newEntities.Add(entity);
                }
            }

            // Delete
            ISet<int> entityIDs = new HashSet<int>(entities.Select(x => x.ID)
                                                           .Where(x => x != 0));

            ISet<int> viewModelIDs = new HashSet<int>(viewModels.Select(x => x.ID)
                                                                .Where(x => x != 0));

            ISet<int> idsToDelete = new HashSet<int>(entityIDs.Except(viewModelIDs));

            // TODO: What happened to set operations on entities thenselves?
            foreach (QuestionLink entity in entities.ToArray())
            {
                if (idsToDelete.Contains(entity.ID))
                {
                    entity.UnlinkRelatedEntities();
                    questionLinkRepository.Delete(entity);
                }
            }

            return newEntities;
        }
    }
}
