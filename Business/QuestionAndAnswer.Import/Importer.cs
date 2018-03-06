using System;
using System.Collections.Generic;
using System.IO;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.Import
{
	/// <summary>
	/// Runs a selector and a converter and returns progress info.
	/// </summary>
	public class Importer<TModel, TSelector, TConverter> : IImporter
		where TSelector : ISelector<TModel>, new()
		where TConverter : ConverterBase<TModel>
	{
		private readonly Source _source;

		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IQuestionCategoryRepository _questionCategoryRepository;
		private readonly IQuestionLinkRepository _questionLinkRepository;
		private readonly IQuestionTypeRepository _questionTypeRepository;
		private readonly ISourceRepository _sourceRepository;
		private readonly IQuestionFlagRepository _questionFlagRepository;

		private Action<string> _progressCallback;
		private Func<bool> _isCancelledCallback;

		private readonly string _categoryIdentifier;

		/// <param name="categoryIdentifier">Defines an extra category to use for the converter.</param>
		public Importer(
			IQuestionRepository questionRepository,
			IAnswerRepository answerRepository,
			ICategoryRepository categoryRepository,
			IQuestionCategoryRepository questionCategoryRepository,
			IQuestionLinkRepository questionLinkRepository,
			IQuestionTypeRepository questionTypeRepository,
			ISourceRepository sourceRepository,
			IQuestionFlagRepository questionFlagRepository,
			Source source,
			string categoryIdentifier)
		{
			_questionRepository = questionRepository ?? throw new NullException(() => questionRepository);
			_answerRepository = answerRepository ?? throw new NullException(() => answerRepository);
			_categoryRepository = categoryRepository ?? throw new NullException(() => categoryRepository);
			_questionCategoryRepository = questionCategoryRepository ?? throw new NullException(() => questionCategoryRepository);
			_questionLinkRepository = questionLinkRepository ?? throw new NullException(() => questionLinkRepository);
			_questionTypeRepository = questionTypeRepository ?? throw new NullException(() => questionTypeRepository);
			_sourceRepository = sourceRepository ?? throw new NullException(() => sourceRepository);
			_questionFlagRepository = questionFlagRepository ?? throw new NullException(() => questionFlagRepository);

			_source = source ?? throw new NullException(() => source);
			_categoryIdentifier = categoryIdentifier;
		}

		public void Execute(string filePath, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
		{
			using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				Execute(stream, progressCallback, isCancelledCallback);
			}
		}

		public void Execute(Stream stream, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
		{
			//try
			//{

			if (stream == null) throw new NullException(() => stream);

			_progressCallback = progressCallback;
			_isCancelledCallback = isCancelledCallback;

			DoProgressCallback("Processing...");

			DeleteExistingQuestions();

			if (!DoImport(stream))
			{
				return;
			}

			DoProgressCallback("Done.");

			//}
			//catch (Exception ex)
			//{
			//	DoProgressCallback(ex.Message);
			//}
		}

		private bool DoImport(Stream stream)
		{
			int counter = 0;

			var selector = new TSelector();
			TConverter converter = CreateConverter();

			foreach (TModel model in selector.GetSelection(stream))
			{
				if (DoIsCancelledCallback())
				{
					DoProgressCallback("Cancelled.");
					return false;
				}

				converter.ConvertToEntities(model);

				counter++;
				DoProgressCallback(string.Format("Processing: {0}", counter));
			}

			return true;
		}

		private TConverter CreateConverter()
		{
			return (TConverter)Activator.CreateInstance(typeof(TConverter),
				_questionRepository,
				_answerRepository,
				_categoryRepository,
				_questionCategoryRepository,
				_questionLinkRepository,
				_questionTypeRepository,
				_sourceRepository,
				_source,
				_categoryIdentifier);
		}

		private void DeleteExistingQuestions()
		{
			foreach (Question question in GetExistingQuestions())
			{
				question.DeleteRelatedEntities(_answerRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository);
				_questionRepository.Delete(question);
			}
		}

		private IList<Question> GetExistingQuestions()
		{
			// _source.ID could be 0 for a new repository, but in that case there are no existing questions anyway and this method will return an empty collection.
			return _questionRepository.GetBySourceID(_source.ID);
		}

		private void DoProgressCallback(string message)
		{
			if (_progressCallback != null) _progressCallback(message);
		}

		private bool DoIsCancelledCallback()
		{
			if (_isCancelledCallback != null)
			{
				return _isCancelledCallback();
			}

			return false;
		}
	}
}
