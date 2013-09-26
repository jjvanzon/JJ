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

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
{
    public class ImportProcess
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss3PropertyIndex;

        private IQuestionRepository _repository;
        private ISourceRepository _sourceRepository;
        private ICategoryRepository _categoryRepository;
        private IAnswerRepository _textualAnswerRepository;
        private IQuestionTypeRepository _questionTypeRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;

        private Action<string> _progressCallback;
        private Func<bool> _isCancelledCallback;

        // TODO: Ask yourself the question whether you want to pass a context at all.
        public ImportProcess(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _repository = new QuestionRepository(context, context.Location);
            _sourceRepository = new SourceRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _textualAnswerRepository = new AnswerRepository(context);
            _questionTypeRepository = new QuestionTypeRepository(context);
            _questionCategoryRepository = new QuestionCategoryRepository(context);
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
                textualQuestion.DeleteRelatedEntities(_textualAnswerRepository, _questionCategoryRepository);
                _repository.Delete(textualQuestion);
            }
        }

        private IEnumerable<Question> GetExistingQuestions()
        {
            return _repository.GetBySource((int)SOURCE);
        }

        private ImportModel GetImportModel(CsvReader reader)
        {
            return new ImportModel
            {
                Name = reader[0],
                Values = reader[1],
                InitialValue = reader[2],
                AppliesTo = reader[3],
                Inherited = reader[4],
                Percentages = reader[5],
                Media = reader[6]
            };
        }

        private void ConvertImportModel(ImportModel importModel)
        {
            TryConvertToQuestion_Values(importModel);
            TryConvertToQuestion_InitialValue(importModel);
            TryConvertToQuestion_AppliesToElements(importModel);
            TryConvertToQuestion_IsInherited(importModel);
            TryConvertToQuestion_Percentages(importModel);
            TryConvertToQuestion_Media(importModel);
        }

        private void TryConvertToQuestion_Values(ImportModel importModel)
        {
            if (!MustConvertToQuestion_Values(importModel.Values))
            {
                return;
            }

            Question question = ConvertToQuestion_BaseMethod();
            if (IsSingular(importModel.Name))
            {
                question.Text = String.Format("What are the possible values for the {0} property?", FormatValue(importModel.Name));
            }
            else
            {
                question.Text = String.Format("What are the possible values for the {0} properties?", FormatValue(importModel.Name));
            }

            if (IsComplexShorthand(importModel))
            {
                question.Text += " (shorthand property)";
            }

            question.Answer().Text = FormatAnswer(importModel.Values);
        }

        private bool MustConvertToQuestion_Values(string answer)
        {
            return !ContainsSee(answer);
        }

        private bool IsComplexShorthand(ImportModel importModel)
        {
            if (importModel.Values == null) return false;

            return importModel.Values.Contains("||") &&
                   importModel.Name != "text-decoration";
        }

        private void TryConvertToQuestion_InitialValue(ImportModel importModel)
        {
            if (!MustConvertToQuestion_InitialValue(importModel.InitialValue))
            {
                return;
            }

            Question question = ConvertToQuestion_BaseMethod();
            if (IsSingular(importModel.Name))
            {
                question.Text = String.Format("What is the initial value of the {0} property?", FormatValue(importModel.Name));
            }
            else
            {
                question.Text = String.Format("What are the initial values of the {0} properties?", FormatValue(importModel.Name));
            }
            question.Answer().Text = FormatAnswer(importModel.InitialValue);
        }

        private bool MustConvertToQuestion_InitialValue(string answer)
        {
            return !ContainsSee(answer);
        }

        private void TryConvertToQuestion_AppliesToElements(ImportModel importModel)
        {
            if (!MustConvertToQuestion_AppliesToElements(importModel.AppliesTo))
            {
                return;
            }

            Question question = ConvertToQuestion_BaseMethod();
            if (IsSingular(importModel.Name))
            {
                question.Text = String.Format("What types of elements does the {0} property apply to?", FormatValue(importModel.Name));
            }
            else
            {
                question.Text = String.Format("What types of elements do the {0} properties apply to?", FormatValue(importModel.Name));
            }

            if (String.IsNullOrWhiteSpace(importModel.AppliesTo))
            {
                question.Answer().Text = "all";
            }
            else
            {
                question.Answer().Text = FormatValue(importModel.AppliesTo);
            }
        }

        private bool MustConvertToQuestion_AppliesToElements(string answer)
        {
            return !ContainsSee(answer);
        }

        private void TryConvertToQuestion_IsInherited(ImportModel importModel)
        {
            if (!MustConvertToQuestion_IsInherited(importModel.Inherited))
            {
                return;
            }

            Question question = ConvertToQuestion_BaseMethod();
            if (IsSingular(importModel.Name))
            {
                question.Text = String.Format("Is the {0} property inherited?", FormatValue(importModel.Name));
            }
            else
            {
                question.Text = String.Format("Are the {0} properties inherited?", FormatValue(importModel.Name));
            }
            question.Answer().Text = FormatAnswer(importModel.Inherited);
        }

        private bool MustConvertToQuestion_IsInherited(string answer)
        {
            return !ContainsSee(answer);
        }

        private void TryConvertToQuestion_Percentages(ImportModel importModel)
        {
            if (!MustConvertToQuestion_Percentages(importModel.Percentages))
            {
                return;
            }

            Question question = ConvertToQuestion_BaseMethod();
            if (IsSingular(importModel.Name))
            {
                question.Text = String.Format("What can you say about percentage values for the {0} property?", FormatValue(importModel.Name));
            }
            else
            {
                question.Text = String.Format("What can you say about percentage values for the {0} properties?", FormatValue(importModel.Name));
            }
            question.Answer().Text = FormatAnswer(importModel.Percentages);
        }

        private bool MustConvertToQuestion_Percentages(string answer)
        {
            return !String.IsNullOrWhiteSpace(answer) &&
                   !ContainsSee(answer);
        }

        private void TryConvertToQuestion_Media(ImportModel importModel)
        {
            if (!MustConvertToQuestion_Media(importModel.Media))
            {
                return;
            }

            Question question = ConvertToQuestion_BaseMethod();
            if (IsSingular(importModel.Name))
            {
                question.Text = String.Format("What media does the {0} property apply to?", FormatValue(importModel.Name));
            }
            else
            {
                question.Text = String.Format("What media do the {0} properties apply to?", FormatValue(importModel.Name));
            }
            question.Answer().Text = FormatAnswer(importModel.Media);
        }

        private bool MustConvertToQuestion_Media(string answer)
        {
            return !ContainsSee(answer);
        }

        private bool IsSingular(string name)
        {
            if (name == null) return true;

            return !name.Trim().Contains(" ");
        }

        // Helpers

        private Question ConvertToQuestion_BaseMethod()
        {
            Question question = _repository.Create();
            question.AutoCreateRelatedEntities(_textualAnswerRepository);
            question.SetSourceEnum(_sourceRepository, SOURCE);
            question.SetQuestionTypeEnum(_questionTypeRepository, QuestionTypeEnum.OpenQuestion);
            question.Answer().IsCorrectAnswer = true;

            QuestionCategory questionCategory = _questionCategoryRepository.Create();
            questionCategory.Question = question;
            questionCategory.SetCategoryEnum(_categoryRepository, CategoryEnum.Css3);

            return question;
        }

        private Dictionary<string, string> _substitutions = new Dictionary<string,string>
        {
            { "<padding-width>" , "<length>, <percentage>"                                                      },
            { "<border-style>"  , "none, hidden, dotted, dashed, solid, double, groove, ridge, inset, outset"   },
            { "<border-width>"  , "thin, medium, thick, <length>"                                               },
            { "<shape>"         , "rect(<top>, <right>, <bottom>, <left>), rect(<length>/auto, <length>/auto, <length>/auto, <length>/auto)"},
            { "<absolute-size>" , "xx-small, x-small, small, medium, large, x-large, xx-large"                  },
            { "<relative-size>" , "larger, smaller"                                                             },
            { "<margin-width>"  , "<length>, <percentage>, auto"                                                },
            { " | "             , ", "                                                                          },
            { " || "            , ", "                                                                          },
            { " ] [ "           , ", "                                                                          },
            { "[ "              , ""                                                                            },
            { " ]"              , ""                                                                            },
            { "["               , ""                                                                            },
            { "]"               , ""                                                                            },
            { "?"               , "(optional)"                                                                  },
            { "+"               , ""                                                                            },
        };

        private string FormatAnswer(string input)
        {
            if (input == null)
            {
                return null;
            }

            string value = input;

            foreach (var x in _substitutions)
            {
                value = value.Replace(x.Key, x.Value);
            }

            value = value.Trim();

            return value;
        }

        private string FormatValue(string value)
        {
            if (value == null) return null;

            return value.Trim();
        }

        private bool ContainsSee(string value)
        {
            Regex containsSeeRegex = new Regex(@"\bsee\b");
            return containsSeeRegex.IsMatch(value);
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
