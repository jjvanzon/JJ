using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Framework.Validation;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;

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
            ISourceRepository sourceRepository,
            Source source,
            string categoryIdentifier)
            : base(questionRepository, answerRepository, categoryRepository, questionCategoryRepository, questionLinkRepository, questionTypeRepository, sourceRepository, source, categoryIdentifier)
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
                question.Text = String.Format("What does the selector {0} mean?", pattern);
            }
            else
            {
                question.Text = String.Format("What do the selectors {0} mean?", pattern);
            }

            question.Answers[0].Text = ImportHelper.TrimValue(model.Meaning);

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConvertToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Selectors", "PatternToMeaning");

            if (!IsPlural(pattern))
            {
                AddCategory(question, "Css3", "Selectors", pattern);
            }
            else
            {
                foreach (string pattern2 in pattern.Split(' '))
                {
                    AddCategory(question, "Css3", "Selectors", pattern2);
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
                question.Text = String.Format("What is the selector for {0}?", meaning);
            }
            else
            {
                question.Text = String.Format("What are the selectors for {0}?", meaning);
            }
            question.Answers[0].Text = pattern;

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConvertToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Selectors", "MeaningToPattern");

            if (!IsPlural(pattern))
            {
                AddCategory(question, "Css3", "Selectors", pattern);
            }
            else
            {
                foreach (string pattern2 in pattern.Split(' '))
                {
                    AddCategory(question, "Css3", "Selectors", pattern2);
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
                question.Text = String.Format("What type of selector is {0} ?", pattern);
            }
            else
            {
                question.Text = String.Format("What type of selector are {0} ?", pattern);
            }
            question.Answers[0].Text = ImportHelper.TrimValue(model.DescribedInSection);

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConvertToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Selectors", "SelectorType");

            if (!IsPlural(pattern))
            {
                AddCategory(question, "Css3", "Selectors", pattern);
            }
            else
            {
                foreach (string pattern2 in pattern.Split(' '))
                {
                    AddCategory(question, "Css3", "Selectors", pattern2);
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

            if (name == "E F" ||
                name == "E > F" ||
                name == "E + F" ||
                name == "E ~ F")
            {
                return false;
            }
            
            return name.Contains(" ");
        }
    }
}
