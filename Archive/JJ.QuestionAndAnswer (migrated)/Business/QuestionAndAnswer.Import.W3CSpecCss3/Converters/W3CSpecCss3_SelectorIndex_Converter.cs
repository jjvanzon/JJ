using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Converters
{
    public class W3CSpecCss3_SelectorIndex_Converter : ConverterBase<W3CSpecCss3_SelectorIndex_ImportModel>
    {
        public W3CSpecCss3_SelectorIndex_Converter(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionTypeRepository questionTypeRepository,
            Source source,
            string categoryPath)
            : base(questionRepository, answerRepository, categoryRepository, questionCategoryRepository, questionLinkRepository, questionTypeRepository, source, categoryPath)
        { }

        public override void ConvertToEntities(W3CSpecCss3_SelectorIndex_ImportModel model)
        {
            ConvertToQuestionFromPatternToMeaning(model);
            ConvertToQuestionFromMeaningToPattern(model);
            ConvertToQuestionAboutSelectorType(model);
        }

        private void ConvertToQuestionFromPatternToMeaning(W3CSpecCss3_SelectorIndex_ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string pattern = ImportHelper.TrimValue(model.Pattern);

            // Set texts
            if (!IsPlural(pattern))
            {
                question.Text = $"What does the selector {pattern} mean?";
            }
            else
            {
                question.Text = $"What do the selectors {pattern} mean?";
            }

            question.Answers[0].Text = ImportHelper.TrimValue(model.Meaning);

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConvertToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            // Add categories
            AutoCreateCategory(question, "Css3", "Selectors", "PatternToMeaning");

            if (!IsPlural(pattern))
            {
                AutoCreateCategory(question, "Css3", "Selectors", pattern);
            }
            else
            {
                foreach (string pattern2 in pattern.Split(' '))
                {
                    AutoCreateCategory(question, "Css3", "Selectors", pattern2);
                }
            }

            // Validate result
            ValidateQuestion(question);
        }

        private void ConvertToQuestionFromMeaningToPattern(W3CSpecCss3_SelectorIndex_ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string meaning = ImportHelper.TrimValue(model.Meaning);
            string pattern = ImportHelper.TrimValue(model.Pattern);

            // Set texts
            if (!IsPlural(pattern))
            {
                question.Text = $"What is the selector for {meaning}?";
            }
            else
            {
                question.Text = $"What are the selectors for {meaning}?";
            }
            question.Answers[0].Text = pattern;

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConvertToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            // Add categories
            AutoCreateCategory(question, "Css3", "Selectors", "MeaningToPattern");

            if (!IsPlural(pattern))
            {
                AutoCreateCategory(question, "Css3", "Selectors", pattern);
            }
            else
            {
                foreach (string pattern2 in pattern.Split(' '))
                {
                    AutoCreateCategory(question, "Css3", "Selectors", pattern2);
                }
            }

            // Validate result
            ValidateQuestion(question);
        }

        private void ConvertToQuestionAboutSelectorType(W3CSpecCss3_SelectorIndex_ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string pattern = ImportHelper.TrimValue(model.Pattern);

            // Set texts
            if (!IsPlural(pattern))
            {
                question.Text = $"What type of selector is {pattern} ?";
            }
            else
            {
                question.Text = $"What type of selector are {pattern} ?";
            }
            question.Answers[0].Text = ImportHelper.TrimValue(model.DescribedInSection);

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConvertToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            // Add categories
            AutoCreateCategory(question, "Css3", "Selectors", "SelectorType");

            if (!IsPlural(pattern))
            {
                AutoCreateCategory(question, "Css3", "Selectors", pattern);
            }
            else
            {
                foreach (string pattern2 in pattern.Split(' '))
                {
                    AutoCreateCategory(question, "Css3", "Selectors", pattern2);
                }
            }

            // Validate result
            ValidateQuestion(question);
        }

        // Helpers

        private bool IsPlural(string name)
        {
            if (name == null) return false;

            name = name.Trim();

            if (string.Equals(name, "E F") ||
                string.Equals(name, "E > F") ||
                string.Equals(name, "E + F") ||
                string.Equals(name, "E ~ F"))
            {
                return false;
            }
            
            return name.Contains(" ");
        }
    }
}
