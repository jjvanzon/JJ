using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex
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
        }

        public void ConvertToEntities(ImportModel model)
        {
            ConvertToQuestion_PatternToMeaning(model);
            ConvertToQuestion_MeaningToPattern(model);
            ConvertToQuestion_SectorType(model);
        }

        private void ConvertToQuestion_PatternToMeaning(ImportModel model)
        {
            Question question = ConvertToQuestion_BaseMethod();
            question.Text = String.Format("What does the selector {0} mean?", FormatValue(model.Pattern));
            question.Answers[0].Text = FormatValue(model.Meaning);

            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConverToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            ValidateQuestion(question);
        }

        private void ConvertToQuestion_MeaningToPattern(ImportModel model)
        {
            Question question = ConvertToQuestion_BaseMethod();
            question.Text = String.Format("What is the selector for {0} ?", FormatValue(model.Meaning));
            question.Answers[0].Text = FormatValue(model.Pattern);

            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConverToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

            ValidateQuestion(question);
        }

        private void ConvertToQuestion_SectorType(ImportModel model)
        {
            Question question = ConvertToQuestion_BaseMethod();
            question.Text = String.Format("What type of selector is {0} ?", FormatValue(model.Pattern));
            question.Answers[0].Text = FormatValue(model.DescribedInSection);

            if (model.DescribedInSectionLink != null)
            {
                QuestionLink link = ConverToLink(model.DescribedInSectionLink);
                link.LinkTo(question);
            }

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

            QuestionCategory questionCategory = _questionCategoryRepository.Create();
            questionCategory.LinkTo(question);
            questionCategory.Question = question;
            questionCategory.SetCategoryEnum(_categoryRepository, CategoryEnum.Css3);

            return question;
        }

        private QuestionLink ConverToLink(LinkModel model)
        {
            QuestionLink link = _questionLinkRepository.Create();
            link.Description = model.Description;
            link.Url = model.Url;
            return link;
        }

        private string FormatValue(string value)
        {
            if (value == null) return null;

            return value.Trim();
        }

        private void ValidateQuestion(Question question)
        {
            var validator = new QuestionDefaultValidator(question);
            validator.Verify();
        }
    }
}
