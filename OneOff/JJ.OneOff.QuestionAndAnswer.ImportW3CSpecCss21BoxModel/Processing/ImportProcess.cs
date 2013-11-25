using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Selectors;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Models;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Processing
{
    public class ImportProcess
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss21BoxModel;

        private IQuestionRepository _questionRepository;
        private IAnswerRepository _answerRepository;
        private ICategoryRepository _categoryRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;
        private IQuestionTypeRepository _questionTypeRepository;
        private ISourceRepository _sourceRepository;

        private Action<string> _progressCallback;
        private Func<bool> _isCancelledCallback;

        public ImportProcess(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionTypeRepository questionTypeRepository,
            ISourceRepository sourceRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionCategoryRepository == null) throw new ArgumentNullException("questionCategoryRepository");
            if (questionLinkRepository == null) throw new ArgumentNullException("questionLinkRepository");
            if (questionTypeRepository == null) throw new ArgumentNullException("questionTypeRepository");
            if (sourceRepository == null) throw new ArgumentNullException("sourceRepository");

            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _categoryRepository = categoryRepository;
            _questionCategoryRepository = questionCategoryRepository;
            _questionLinkRepository = questionLinkRepository;
            _questionTypeRepository = questionTypeRepository;
            _sourceRepository = sourceRepository;
        }

        public void Execute(string filePath, bool includeAnswersThatAreReferences, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Execute(stream, includeAnswersThatAreReferences, progressCallback, isCancelledCallback);
            }
        }

        public void Execute(Stream stream, bool includeAnswersThatAreReferences, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null)
        {
            //try
            //{

            if (stream == null) throw new ArgumentNullException("stream");

            _progressCallback = progressCallback;
            _isCancelledCallback = isCancelledCallback;

            DoProgressCallback("Processing...");

            DeleteExistingQuestions();

            stream.Position = 0;
            if (!ImportPropertyDefinitions(stream))
            {
                return;
            }

            /*stream.Position = 0;
            if (!ImportDefinitions(stream))
            {
                return;
            }*/

            CorrectCategoryDescriptions();

            DoProgressCallback("Done.");

            //}
            //catch (Exception ex)
            //{
            //    DoProgressCallback(ex.Message);
            //}
        }

        private bool ImportPropertyDefinitions(Stream stream)
        {
            int counter = 0;

            var selector = new PropertyDefinitionSelector();
            var converter = CreatePropertyDefinitionConverter();

            foreach (PropertyDefinitionModel model in selector.GetSelection(stream))
            {
                if (DoIsCancelledCallback())
                {
                    DoProgressCallback("Cancelled.");
                    return false;
                }

                converter.ConvertToEntities(model);

                counter++;
                DoProgressCallback(String.Format("Processing property definitions: {0}", counter));
            }

            return true;
        }

        private PropertyDefinitionConverter CreatePropertyDefinitionConverter()
        {
            return new PropertyDefinitionConverter(
                _questionRepository,
                _answerRepository,
                _categoryRepository,
                _questionCategoryRepository,
                _questionLinkRepository,
                _questionTypeRepository,
                _sourceRepository);
        }

        private bool ImportDefinitions(Stream stream)
        {
            int counter = 0;

            var selector = new DefinitionSelector();
            var converter = CreateDefinitionConverter();

            foreach (DefinitionModel model in selector.GetSelection(stream))
            {
                if (DoIsCancelledCallback())
                {
                    DoProgressCallback("Cancelled.");
                    return false;
                }

                converter.ConvertToEntities(model);

                counter++;
                DoProgressCallback(String.Format("Processing definitions: {0}", counter));
            }

            return true;
        }

        private DefinitionConverter CreateDefinitionConverter()
        {
            return new DefinitionConverter(
                _questionRepository,
                _answerRepository,
                _categoryRepository,
                _questionCategoryRepository,
                _questionLinkRepository,
                _questionTypeRepository,
                _sourceRepository);
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
            CorrectCategoryDescription("BoxModel", "Box Model");
            CorrectCategoryDescription("PossibleValues", "Possible Values");
            CorrectCategoryDescription("InitialValue", "Initial Value");
            CorrectCategoryDescription("AppliesToElements", "Applies to Elements");
            CorrectCategoryDescription("IsInherited", "Inherit");
            CorrectCategoryDescription("ComputedValue", "Computed Value");
            CorrectCategoryDescription("TermToMeaning", "Term to Meaning");
            CorrectCategoryDescription("MeaningToTerm", "Meaning to Term");
        }

        private void CorrectCategoryDescription(string identifier, string description)
        {
            Category category = _categoryRepository.TryGetByIdentifier(identifier);
            if (category == null)
            {
                // Commented out to allow partial imports that may not create the category.
                // TODO: Consider uncommenting this again.
                //throw new Exception(String.Format("Category with Identifier '{0}' not found.", identifier));
                return;
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
