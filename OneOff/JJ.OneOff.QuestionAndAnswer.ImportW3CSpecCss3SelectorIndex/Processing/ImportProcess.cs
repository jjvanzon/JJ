using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JJ.Framework.IO;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Enums;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Selectors;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Models;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Processing
{
    public class ImportProcess
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss3SelectorIndex;

        private IContext _context;

        private IQuestionRepository _questionRepository;
        private ISourceRepository _sourceRepository;
        private ICategoryRepository _categoryRepository;
        private IAnswerRepository _answerRepository;
        private IQuestionTypeRepository _questionTypeRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;

        private Action<string> _progressCallback;
        private Func<bool> _isCancelledCallback;

        // TODO: Ask yourself the question whether you want to pass a context at all.
        public ImportProcess(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;

            _questionRepository = new QuestionRepository(context, context.Location);
            _sourceRepository = new SourceRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _answerRepository = new AnswerRepository(context);
            _questionTypeRepository = new QuestionTypeRepository(context);
            _questionCategoryRepository = new QuestionCategoryRepository(context);
            _questionLinkRepository = new QuestionLinkRepository(context);
        }

        public void Execute(string filePath, ImportTypeEnum importType, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Execute(stream, importType, progressCallback, isCancelledCallback);
            }
        }

        public void Execute(Stream stream, ImportTypeEnum importType, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            //try
            //{

            if (stream == null) throw new ArgumentNullException("stream");
            _progressCallback = progressCallback;
            _isCancelledCallback = isCancelledCallback;

            DoProgressCallback("Processing...");

            DeleteExistingQuestions();

            var selector = SelectorFactory.CreateSelector(importType);
            var converter = new ImportModelConverter(_context);

            int counter = 0;

            foreach (ImportModel model in selector.GetSelection(stream))
            {
                if (DoIsCancelledCallback())
                {
                    DoProgressCallback("Cancelled.");
                    return;
                }

                converter.ConvertToEntities(model);

                counter++;
                DoProgressCallback(String.Format("Processing: {0}", counter));
            }

            CorrectCategoryDescriptions();

            _questionRepository.Commit();

            DoProgressCallback("Done.");

            //}
            //catch (Exception ex)
            //{
            //    DoProgressCallback(ex.Message);
            //}
        }

        private void DeleteExistingQuestions()
        {
            foreach (Question question in GetExistingQuestions())
            {
                question.DeleteRelatedEntities(_answerRepository, _questionCategoryRepository, _questionLinkRepository);
                _questionRepository.Delete(question);
            }
        }

        private IEnumerable<Question> GetExistingQuestions()
        {
            return _questionRepository.GetBySource((int)SOURCE);
        }

        private void CorrectCategoryDescriptions()
        {
            CorrectCategoryDescription("Css3", "CSS3");
            CorrectCategoryDescription("PatternToMeaning", "Pattern to Meaning");
            CorrectCategoryDescription("MeaningToPattern", "Meaning to Pattern");
            CorrectCategoryDescription("SelectorType", "Selector Type");
        }

        private void CorrectCategoryDescription(string identifier, string description)
        {
            Category category = _categoryRepository.TryGetByIdentifier(identifier);
            if (category == null)
            {
                throw new Exception(String.Format("Category with Identifier '{0}' not found.", identifier));
            }
            category.Description = description;
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
