using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Business.QuestionAndAnswer.SideEffects;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Text;
using JJ.Framework.Validation;
using static JJ.Framework.Reflection.ExpressionHelper;
// ReSharper disable MemberCanBePrivate.Global

namespace JJ.Business.QuestionAndAnswer.Import
{
	public abstract class ConverterBase<TModel>
	{
		protected readonly Source _source;

		protected readonly EntityStatusManager _entityStatusManager;

		protected readonly IQuestionRepository _questionRepository;
		protected readonly IAnswerRepository _answerRepository;
		protected readonly ICategoryRepository _categoryRepository;
		protected readonly IQuestionCategoryRepository _questionCategoryRepository;
		protected readonly IQuestionLinkRepository _questionLinkRepository;
		protected readonly IQuestionTypeRepository _questionTypeRepository;

		private readonly CategoryManager _categoryManager;

		protected readonly string[] _categoryIdentifiers;

		/// <param name="categoryPath">Defines an extra category to use for this specific converter.</param>
		public ConverterBase(
			IQuestionRepository questionRepository,
			IAnswerRepository answerRepository,
			ICategoryRepository categoryRepository,
			IQuestionCategoryRepository questionCategoryRepository,
			IQuestionLinkRepository questionLinkRepository,
			IQuestionTypeRepository questionTypeRepository,
			Source source,
			string categoryPath)
		{
			_questionRepository = questionRepository ?? throw new NullException(() => questionRepository);
			_answerRepository = answerRepository ?? throw new NullException(() => answerRepository);
			_categoryRepository = categoryRepository ?? throw new NullException(() => categoryRepository);
			_questionCategoryRepository = questionCategoryRepository ?? throw new NullException(() => questionCategoryRepository);
			_questionLinkRepository = questionLinkRepository ?? throw new NullException(() => questionLinkRepository);
			_questionTypeRepository = questionTypeRepository ?? throw new NullException(() => questionTypeRepository);

			_entityStatusManager = new EntityStatusManager();

			_categoryManager = new CategoryManager(_categoryRepository);

			_categoryIdentifiers = (categoryPath ?? "").Split("\\").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

			_source = source ?? throw new NullException(() => source);
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

		protected void AutoCreateCategory(Question question, IEnumerable<string> categoryIdentifiers)
			=> AutoCreateCategory(question, categoryIdentifiers.ToArray());

		protected void AutoCreateCategory(Question question, params string[] categoryIdentifiers)
			=> AutoCreateCategory(question, (IList<string>)categoryIdentifiers);

		protected void AutoCreateCategory(Question question, IList<string> categoryIdentifiers)
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
			value = value?.Trim();

			// Some terms have < > around them, and sometimes even '< and >'. Those are ugly in a link.
			value = value?.Replace("<'", "")
			             .Replace("'>", "")
			             .Replace(">", "")
			             .Replace("<", "")
			             .TrimStart("'")
			             .TrimEnd("'");

			return value;
		}

		/// <summary> Turns relative url's to absolute urls. </summary>
		private string ResolveLinkUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new NullOrEmptyException(() => url);
			}

			// Absolute URL e.g. "http://www.bla.com/something/page.html"
			if (url.StartsWith("http://") ||
			    url.StartsWith("https://"))
			{
				return url;
			}

			// An internal link e.g. "#mysection"
			if (url.StartsWith("#"))
			{
				return _source.Url + url;
			}

			// A relative link to another page on the same site e.g. "anotherpage.html#mysection"
			// Source url can be e.g.
			// - "http://www.w3.org/TR/CSS21/tables.html#table-display"
			// - "http://www.w3.org/TR/CSS21/tables.html"
			// - "http://www.w3.org/TR/CSS21/tables.bla/"

			// It is assumed that it either ends with a slash, or you have to cut off everything until the slash.
			if (string.IsNullOrWhiteSpace(_source.Url))
			{
				throw new Exception(
					$"{new { url }} was considered a relative URL, but those do not work if if {GetText(() => _source.Url)} is empty.");
			}

			return _source.Url.TrimEndUntil("/") + url;
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