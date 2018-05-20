using System.Globalization;
using System.Web;
using JetBrains.Annotations;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Text;

namespace JJ.Business.QuestionAndAnswer.Import.Language
{
	[UsedImplicitly]
	public class Converter : ConverterBase<Model>
	{
		public Converter(
			IQuestionRepository questionRepository,
			IAnswerRepository answerRepository,
			ICategoryRepository categoryRepository,
			IQuestionCategoryRepository questionCategoryRepository,
			IQuestionLinkRepository questionLinkRepository,
			IQuestionTypeRepository questionTypeRepository,
			Source source,
			string categoryIdentifier)
			: base(
				questionRepository,
				answerRepository,
				categoryRepository,
				questionCategoryRepository,
				questionLinkRepository,
				questionTypeRepository,
				source,
				categoryIdentifier)
		{
			// Category identifier not pemitted in this implementation
			if (!string.IsNullOrEmpty(categoryIdentifier)) throw new NotNullOrEmptyException(() => categoryIdentifier);
		}

		public override void ConvertToEntities(Model model)
		{
			ConvertToQuestion(model.CultureCodeA, model.WordInCultureA, model.CultureCodeB, model.WordInCultureB);
			ConvertToQuestion(model.CultureCodeB, model.WordInCultureB, model.CultureCodeA, model.WordInCultureA);
		}

		private void ConvertToQuestion(string cultureCodeA, string wordInCultureA, string cultureCodeB, string wordInCultureB)
		{
			// Initialize some variables
			var cultureA = new CultureInfo(cultureCodeA);
			var cultureB = new CultureInfo(cultureCodeB);
			string friendlyLanguageNameA = cultureA.EnglishName.TakeStartUntil(" (");
			string friendlyLanguageNameB = cultureB.EnglishName.TakeStartUntil(" (");
			string googleLanguageCodeA = cultureA.TwoLetterISOLanguageName;
			string googleLanguageCodeB = cultureB.TwoLetterISOLanguageName;

			// Create question
			Question question = ConvertToQuestion_BaseMethod();

			// Set texts
			question.Text = wordInCultureA?.Trim();
			question.Answers[0].Text = wordInCultureB?.Trim();

			// Create Link
			string encodedGoogleLanguageCodeA = HttpUtility.UrlEncode(googleLanguageCodeA);
			string encodedGoogleLanguageCodeB = HttpUtility.UrlEncode(googleLanguageCodeB);
			string encodedWordInCultureA = HttpUtility.UrlEncode(wordInCultureA);
			QuestionLink questionLink = ConvertToLink(new LinkModel
			{
				Description = "Google Translate",
				Url = $"https://translate.google.nl/#{encodedGoogleLanguageCodeA}/{encodedGoogleLanguageCodeB}/{encodedWordInCultureA}"
			});
			questionLink.LinkTo(question);

			// Add categories
			AddCategory(question, "Language", $"{friendlyLanguageNameA}To{friendlyLanguageNameB}");

			// Validate result
			ValidateQuestion(question);
		}
	}
}