using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Framework.Reflection.Exceptions;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;

namespace JJ.Presentation.QuestionAndAnswer.ToEntity
{
    internal static class QuestionEditViewModelExtensions
    {
        public static Question ToEntity(this QuestionEditViewModel viewModel, Repositories repositories)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (repositories == null) throw new NullException(() => repositories);

            viewModel.NullCoalesce();

            // Question
            Question question = viewModel.Question.ToEntity(repositories.QuestionRepository, repositories.AnswerRepository, repositories.SourceRepository, repositories.QuestionTypeRepository, repositories.EntityStatusManager);

            // Answer
            // For now multiple answers are not supported.
            Answer answer = viewModel.Question.ToAnswer(repositories.AnswerRepository, repositories.EntityStatusManager);
            question.LinkTo(answer);

            // Flags
            foreach (QuestionFlagViewModel questionFlagViewModel in viewModel.Question.Flags)
            {
                QuestionFlag questionFlag2 = questionFlagViewModel.ToEntity(repositories.QuestionFlagRepository, repositories.FlagStatusRepository);
                questionFlag2.LinkTo(question);
            }

            // Categories
            IList<QuestionCategory> newQuestionCategories = ListViewModelConverter.ConvertQuestionCategories(viewModel.Question.Categories, question.QuestionCategories, repositories.QuestionCategoryRepository, repositories.CategoryRepository, repositories.EntityStatusManager);
            foreach (QuestionCategory newQuestionCategory in newQuestionCategories)
            {
                newQuestionCategory.LinkTo(question);
            }

            // Links
            IList<QuestionLink> newQuestionLinks = ListViewModelConverter.ConvertQuestionLinks(viewModel.Question.Links, question.QuestionLinks, repositories.QuestionLinkRepository, repositories.EntityStatusManager);
            foreach (QuestionLink newQuestionLink in newQuestionLinks)
            {
                newQuestionLink.LinkTo(question);
            }

            return question;
        }
    }
}
