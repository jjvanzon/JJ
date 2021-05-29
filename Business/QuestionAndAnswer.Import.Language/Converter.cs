using System.Globalization;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
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
            string categoryPath)
            : base(
                questionRepository,
                answerRepository,
                categoryRepository,
                questionCategoryRepository,
                questionLinkRepository,
                questionTypeRepository,
                source,
                categoryPath)
        { }

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
            string[] formattedCategoryIdentifiers = _categoryIdentifiers
                                                    .Select(x => x.Replace("{0}", friendlyLanguageNameA).Replace("{1}", friendlyLanguageNameB))
                                                    .ToArray();

            AutoCreateCategory(question, formattedCategoryIdentifiers);

            // Validate result
            ValidateQuestion(question);
        }
    }
}