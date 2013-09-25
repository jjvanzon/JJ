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

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex
{
    public class ImportProcess
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss3SelectorIndex;

        private ITextualQuestionRepository _repository;
        private ISourceRepository _sourceRepository;
        private ICategoryRepository _categoryRepository;
        private ITextualAnswerRepository _textualAnswerRepository;

        private Action<string> _progressCallback;
        private Func<bool> _isCancelledCallback;

        // TODO: Ask yourself the question whether you want to pass a context at all.
        public ImportProcess(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _repository = new TextualQuestionRepository(context, context.Location);
            _sourceRepository = new SourceRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _textualAnswerRepository = new TextualAnswerRepository(context);
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
            try
            {
                if (stream == null) throw new ArgumentNullException("stream");
                _progressCallback = progressCallback;
                _isCancelledCallback = isCancelledCallback;

                DoProgressCallback("Processing...");

                DeleteExistingQuestions();

                using (CsvReader reader = new CsvReader(stream))
                {
                    // Skip header.
                    reader.Read();
                    ImportModel header = GetImportModel(reader);

                    int counter = 0;

                    while (reader.Read())
                    {
                        // Cancel
                        if (DoIsCancelledCallback())
                        {
                            DoProgressCallback("Cancelled.");
                            return;
                        }

                        // Process record
                        ImportModel importModel = GetImportModel(reader);
                        ConvertImportModel(importModel);

                        // Progress
                        counter++;
                        DoProgressCallback(String.Format("Processing: {0}", counter));
                    }
                }
                
                _repository.Commit();

                DoProgressCallback("Done.");
            }
            catch (Exception ex)
            {
                DoProgressCallback(ex.Message);
            }
        }

        private void DeleteExistingQuestions()
        {
            foreach (TextualQuestion textualQuestion in GetExistingQuestions())
            {
                textualQuestion.DeleteRelatedEntities(_textualAnswerRepository);
                _repository.Delete(textualQuestion);
            }
        }

        private IEnumerable<TextualQuestion> GetExistingQuestions()
        {
            return _repository.GetBySource((int)SOURCE);
        }

        private ImportModel GetImportModel(CsvReader reader)
        {
            return new ImportModel
            {
                Pattern = reader[0],
                Meaning = reader[1],
                DescribedInSection = reader[2],
                FirstDefinedInLevel = reader[3]
            };
        }

        private void ConvertImportModel(ImportModel importModel)
        {
            ConvertToQuestion_PatternToMeaning(importModel);
            ConvertToQuestion_MeaningToPattern(importModel);
            ConvertToQuestion_SectorType(importModel);
        }

        private void ConvertToQuestion_PatternToMeaning(ImportModel importModel)
        {
            TextualQuestion question = ConvertToQuestion_BaseMethod();
            question.Text = String.Format("What does the selector {0} mean?", FormatValue(importModel.Pattern));
            question.TextualAnswer().Text = FormatValue(importModel.Meaning);
        }

        private void ConvertToQuestion_MeaningToPattern(ImportModel importModel)
        {
            TextualQuestion question = ConvertToQuestion_BaseMethod();
            question.Text = String.Format("What is the selector for {0} ?", FormatValue(importModel.Meaning));
            question.TextualAnswer().Text = FormatValue(importModel.Pattern);
        }

        private void ConvertToQuestion_SectorType(ImportModel importModel)
        {
            TextualQuestion question = ConvertToQuestion_BaseMethod();
            question.Text = String.Format("What type of selector is {0} ?", FormatValue(importModel.Pattern));
            question.TextualAnswer().Text = FormatValue(importModel.DescribedInSection);
        }

        // Helpers

        private TextualQuestion ConvertToQuestion_BaseMethod()
        {
            TextualQuestion question = _repository.Create();
            question.AutoCreateRelatedEntities(_textualAnswerRepository);
            question.SetSourceValue(_sourceRepository, SOURCE);
            question.SetCategoryValue(_categoryRepository, CategoryEnum.Css3);
            return question;
        }

        private string FormatValue(string value)
        {
            if (value == null) return null;

            return value.Trim();
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
