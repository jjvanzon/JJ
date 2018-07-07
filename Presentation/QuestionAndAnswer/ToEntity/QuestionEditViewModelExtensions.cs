using System.Collections.Generic;
using JJ.Data.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Exceptions.Basic;
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
			Question question = viewModel.Question.ToEntity(repositories.QuestionRepository, repositories.SourceRepository, repositories.QuestionTypeRepository, repositories.EntityStatusManager);

			// Answer
			// For now multiple answers are not supported.
			Answer answer = viewModel.Question.ToAnswer(repositories.AnswerRepository, repositories.EntityStatusManager);
			question.LinkTo(answer);

			// Flags
			foreach (QuestionFlagViewModel questionFlagViewModel in viewModel.Question.Flags)
			{
				QuestionFlag questionFlag = questionFlagViewModel.ToEntity(repositories.QuestionFlagRepository, repositories.FlagStatusRepository, repositories.EntityStatusManager);
				questionFlag.LinkTo(question);
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
