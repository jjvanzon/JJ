using System;
using JJ.Framework.Common;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Exceptions;
using JJ.Framework.Business;
using JJ.Business.QuestionAndAnswer.SideEffects;

namespace JJ.Business.QuestionAndAnswer.Import
{
    public abstract class ConverterBase<TModel>
    {
        protected readonly Source _source;

        protected EntityStatusManager _entityStatusManager;

        protected IQuestionRepository _questionRepository;
        protected IAnswerRepository _answerRepository;
        protected ICategoryRepository _categoryRepository;
        protected IQuestionCategoryRepository _questionCategoryRepository;
        protected IQuestionLinkRepository _questionLinkRepository;
        protected IQuestionTypeRepository _questionTypeRepository;
        protected ISourceRepository _sourceRepository;

        private CategoryManager _categoryManager;

        protected string _categoryIdentifier;

        /// <param name="categoryIdentifier">Defines an extra category to use for this specific converter.</param>
        public ConverterBase(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionTypeRepository questionTypeRepository,
            ISourceRepository sourceRepository,
            Source source,
            string categoryIdentifier)
        {
            if (questionRepository == null) throw new NullException(() => questionRepository);
            if (answerRepository == null) throw new NullException(() => answerRepository);
            if (categoryRepository == null) throw new NullException(() => categoryRepository);
            if (questionCategoryRepository == null) throw new NullException(() => questionCategoryRepository);
            if (questionLinkRepository == null) throw new NullException(() => questionLinkRepository);
            if (questionTypeRepository == null) throw new NullException(() => questionTypeRepository);
            if (sourceRepository == null) throw new NullException(() => sourceRepository);
            if (source == null) throw new NullException(() => source);

            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _categoryRepository = categoryRepository;
            _questionCategoryRepository = questionCategoryRepository;
            _questionLinkRepository = questionLinkRepository;
            _questionTypeRepository = questionTypeRepository;
            _sourceRepository = sourceRepository;

            _entityStatusManager = new EntityStatusManager();

            _categoryManager = new CategoryManager(_categoryRepository);

            _categoryIdentifier = categoryIdentifier;

            _source = source;
        }

        public abstract void ConvertToEntities(TModel model);

        protected Question ConvertToQuestion_BaseMethod()
        {
            Question question = _questionRepository.Create();
            _entityStatusManager.SetIsNew(question);

            ISideEffect sideEffect1 = new Question_SideEffect_AutoCreateRelatedEntities(question, _answerRepository, _entityStatusManager);
            sideEffect1.Execute();

            ISideEffect sideEffect2 = new Question_SideEffect_SetDefaults_ForOpenQuestion(question, _questionTypeRepository, _entityStatusManager);
            sideEffect2.Execute();

            ISideEffect sideEffect3 = new Answer_SideEffect_SetDefaults_ForOpenQuestion(question.Answers[0], _entityStatusManager);
            sideEffect3.Execute();

            question.LinkTo(_source);

            return question;
        }

        protected void AddCategory(Question question, params string[] categoryIdentifiers)
        {
            Category category = _categoryManager.FindOrCreateCategoryByIdentifierPath(categoryIdentifiers);
            QuestionCategory questionCategory = _questionCategoryRepository.Create();
            questionCategory.LinkTo(question);
            questionCategory.LinkTo(category);
        }

        protected QuestionLink ConvertToLink(LinkModel model)
        {
            QuestionLink link = _questionLinkRepository.Create();
            link.Description = FormatLinkDescription(model.Description);
            link.Url = ResolveLinkUrl(model.Url);
            return link;
        }

        /// <summary> Trims, cuts off &lt; from the beginning and &gt; from the end and removes surrounding single quotes ('). </summary>
        private string FormatLinkDescription(string value)
        {
            // Some link descriptions are terms, that are sometimes surrounded with < and >, and sometimes with single quotes (').
            value = TrimValue(value);

            // Some terms have < > around them, and sometimes even '< and >'. Those are ugly in a link.
            if (value != null)
            {
                value = value.Replace("<'", "")
                             .Replace("'>", "")
                             .Replace(">", "")
                             .Replace("<", "")
                             .CutLeft("'")
                             .CutRight("'");
            }

            // Older (?) version.
            /*value = value.CutLeft("<");
            value = value.CutRight(">");
            value = value.CutLeft("'");
            value = value.CutRight("'");*/
            return value;
        }

        /// <summary> Turns relative url's to absolute urls. </summary>
        private string ResolveLinkUrl(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new Exception("url cannot be null or empty.");
            }

            // Absolute URL e.g. "http://www.bla.com/something/page.html"
            if (url.StartsWith("http://"))
            {
                return url;
            }

            // An internal link e.g. "#mysection"
            if (url.StartsWith("#"))
            {
                return _source.Url + url;
            }

            // A relative link to another page on the same site e.g. "anotherpage.html#mysection"
            else
            {
                // Source url can be e.g.
                // - "http://www.w3.org/TR/CSS21/tables.html#table-display"
                // - "http://www.w3.org/TR/CSS21/tables.html"
                // - "http://www.w3.org/TR/CSS21/tables.bla/"

                // It is assumed that it either ends with a slash, or you have to cut off everything until the slash.
                return _source.Url.CutRightUntil("/") + url;
            }
        }

        /// <summary> Trims, but does not throw exception when value is null. </summary>
        private string TrimValue(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Trim();
        }

        protected void ValidateQuestion(Question question)
        {
            IValidator validator1 = new BasicQuestionValidator(question);
            IValidator validator2 = new OpenQuestionValidator(question);
            validator1.Assert();
            validator2.Assert();
        }
    }
}
