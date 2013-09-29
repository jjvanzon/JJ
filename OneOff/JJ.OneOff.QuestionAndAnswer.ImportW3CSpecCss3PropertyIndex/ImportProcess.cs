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
using JJ.Business.QuestionAndAnswer.Validation;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
{
    public class ImportProcess
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss3PropertyIndex;

        private IContext _context;
        private IAnswerRepository _answerRepository;
        private IQuestionRepository _questionRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;
        private ICategoryRepository _categoryRepository;

        private Action<string> _progressCallback;
        private Func<bool> _isCancelledCallback;

        // TODO: Ask yourself the question whether you want to pass a context at all.
        public ImportProcess(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
            _questionRepository = new QuestionRepository(context, context.Location);
            _answerRepository = new AnswerRepository(context);
            _questionCategoryRepository = new QuestionCategoryRepository(context);
            _questionLinkRepository = new QuestionLinkRepository(context);
            _categoryRepository = new CategoryRepository(context);
        }

        public void Execute(string filePath, ImportTypeEnum importType, bool includeAnswersThatAreReferences, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Execute(stream, importType, includeAnswersThatAreReferences, progressCallback, isCancelledCallback);
            }
        }

        public void Execute(Stream stream, ImportTypeEnum importType, bool includeAnswersThatAreReferences, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            //try
            //{

            if (stream == null) throw new ArgumentNullException("stream");

            _progressCallback = progressCallback;
            _isCancelledCallback = isCancelledCallback;

            DoProgressCallback("Processing...");

            DeleteExistingQuestions();

            int counter = 0;

            var selector = SelectorFactory.CreateSelector(importType);
            var converter = new ImportModelConverter(_context, includeAnswersThatAreReferences);

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

            _context.Commit();

            DoProgressCallback("Done.");

            //}
            //catch (Exception ex)
            //{
            //    DoProgressCallback(ex.Message);
            //}
        }

        private void DeleteExistingQuestions()
        {
            foreach (Question textualQuestion in GetExistingQuestions())
            {
                textualQuestion.DeleteRelatedEntities(_answerRepository, _questionCategoryRepository, _questionLinkRepository);
                _questionRepository.Delete(textualQuestion);
            }
        }

        private IEnumerable<Question> GetExistingQuestions()
        {
            return _questionRepository.GetBySource((int)SOURCE);
        }

        private void CorrectCategoryDescriptions()
        {
            CorrectCategoryDescription("Css3", "CSS3");
            CorrectCategoryDescription("InitialValue", "Initial Value");
            CorrectCategoryDescription("AppliesToElements", "Applies to Elements");
            CorrectCategoryDescription("IsInherited", "Inherit");
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
