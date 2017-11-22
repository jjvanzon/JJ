using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System.Collections.Generic;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
	public sealed class RandomQuestionViewModel
	{
		public LoginPartialViewModel Login { get; set; }
		public LanguageSelectorPartialViewModel LanguageSelector { get; set; }

		public QuestionViewModel Question { get; set; }

		public bool AnswerIsVisible { get; set; }
		public string UserAnswer { get; set; }

		public CurrentUserQuestionFlagPartialViewModel CurrentUserQuestionFlag { get; set; }

		/// <summary>
		/// Used internally for selecting the next question.
		/// </summary>
		public IList<CategoryViewModel> SelectedCategories { get; set; }
	}
}
