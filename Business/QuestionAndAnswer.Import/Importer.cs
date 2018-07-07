using System;
using System.Collections.Generic;
using System.IO;
using JJ.Business.QuestionAndAnswer.Cascading;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.Import
{
	/// <summary>
	/// Runs a selector and a converter and returns progress info.
	/// Also deletes the questions of a certain source first.
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

			DeleteExistingQuestionsOfSource();

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
				DoProgressCallback($"Processing: {counter}");
			}

			return true;
		}

		private TConverter CreateConverter()
			=> (TConverter)Activator.CreateInstance(
				typeof(TConverter),
				_questionRepository,
				_answerRepository,
				_categoryRepository,
				_questionCategoryRepository,
				_questionLinkRepository,
				_questionTypeRepository,
				_source,
				_categoryIdentifier);

		private void DeleteExistingQuestionsOfSource()
		{
			foreach (Question question in GetExistingQuestionsOfSource())
			{
				question.DeleteRelatedEntities(_answerRepository, _questionCategoryRepository, _questionLinkRepository, _questionFlagRepository);
				_questionRepository.Delete(question);
			}
		}

		private IList<Question> GetExistingQuestionsOfSource() => _questionRepository.GetBySourceID(_source.ID);

		private void DoProgressCallback(string message) => _progressCallback?.Invoke(message);

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