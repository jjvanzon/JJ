using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Models;
using JJ.Framework.Validation;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Processing
{
    public class ImportModelConverter
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss3SelectorIndex;

        private IQuestionRepository _questionRepository;
        private ISourceRepository _sourceRepository;
        private ICategoryRepository _categoryRepository;
        private IAnswerRepository _answerRepository;
        private IQuestionTypeRepository _questionTypeRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;

        private CategoryManager _categoryManager;

        public ImportModelConverter(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _questionRepository = new QuestionRepository(context, context.Location);
            _sourceRepository = new SourceRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _answerRepository = new AnswerRepository(context);
            _questionTypeRepository = new QuestionTypeRepository(context);
            _questionCategoryRepository = new QuestionCategoryRepository(context);
            _questionLinkRepository = new QuestionLinkRepository(context);

            _categoryManager = new CategoryManager(_categoryRepository);
        }

        public void ConvertToEntities(ImportModel model)
        {
            ConvertToQuestionFromPatternToMeaning(model);
            ConvertToQuestionFromMeaningToPattern(model);
            ConvertToQuestionAboutSelectorType(model);
        }

        private void ConvertToQuestionFromPatternToMeaning(ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string pattern = FormatValue(model.Pattern);

            // Set texts
            if (!IsPlural(pattern))
            {
                question.Text = String.Format("What does the selector {0} mean?", pattern);
            }
            else
            {
                question.Text = String.Format("What do the selectors {0} mean?", pattern);
            }

            question.Answers[0].Text = FormatValue(model.Meaning);

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConverToLink(model.DescribedInSectionLink);
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

        private void ConvertToQuestionFromMeaningToPattern(ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string meaning = FormatValue(model.Meaning);
            string pattern = FormatValue(model.Pattern);

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
                QuestionLink link = ConverToLink(model.DescribedInSectionLink);
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

        private void ConvertToQuestionAboutSelectorType(ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string pattern = FormatValue(model.Pattern);

            // Set texts
            if (!IsPlural(pattern))
            {
                question.Text = String.Format("What type of selector is {0} ?", pattern);
            }
            else
            {
                question.Text = String.Format("What type of selector are {0} ?", pattern);
            }
            question.Answers[0].Text = FormatValue(model.DescribedInSection);

            // Create links
            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConverToLink(model.DescribedInSectionLink);
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

        private Question ConvertToQuestion_BaseMethod()
        {
            Question question = _questionRepository.Create();
            question.AutoCreateRelatedEntities(_answerRepository);
            question.SetSourceEnum(_sourceRepository, SOURCE);
            question.SetQuestionTypeEnum(_questionTypeRepository, QuestionTypeEnum.OpenQuestion);
            question.Answers[0].IsCorrectAnswer = true;
            return question;
        }

        private QuestionLink ConverToLink(LinkModel model)
        {
            QuestionLink link = _questionLinkRepository.Create();
            link.Description = model.Description;
            link.Url = model.Url;
            return link;
        }

        private void AddCategory(Question question, params string[] categoryIdentifiers)
        {
            Category category = _categoryManager.FindOrCreateCategoryByIdentifierPath(categoryIdentifiers);
            QuestionCategory questionCategory = _questionCategoryRepository.Create();
            questionCategory.LinkTo(question);
            questionCategory.LinkTo(category);
        }

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

        private string FormatValue(string value)
        {
            if (value == null) return null;

            return value.Trim();
        }

        private void ValidateQuestion(Question question)
        {
            IValidator validator = new QuestionOpenQuestionValidator(question);
            validator.Verify();
        }
    }
}
