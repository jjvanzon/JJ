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
using JJ.Business.QuestionAndAnswer;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
{
    public class ImportModelConverter
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss3PropertyIndex;

        private IQuestionRepository _questionRepository;
        private ISourceRepository _sourceRepository;
        private ICategoryRepository _categoryRepository;
        private IAnswerRepository _answerRepository;
        private IQuestionTypeRepository _questionTypeRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;

        private CategoryManager _categoryManager;

        private bool _includeAnswersThatAreReferences;

        public ImportModelConverter(IContext context, bool includeAnswersThatAreReferences)
        {
            if (context == null) throw new ArgumentNullException("context");

            _questionRepository = new QuestionRepository(context, context.Location);
            _answerRepository = new AnswerRepository(context);
            _sourceRepository = new SourceRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _questionTypeRepository = new QuestionTypeRepository(context);
            _questionCategoryRepository = new QuestionCategoryRepository(context);
            _questionLinkRepository = new QuestionLinkRepository(context);

            _categoryManager = new CategoryManager(_categoryRepository);

            _includeAnswersThatAreReferences = includeAnswersThatAreReferences;
        }

        public void ConvertToEntities(ImportModel model)
        {
            TryConvertToQuestionAboutValues(model);
            TryConvertToQuestionAboutInitialValue(model);
            TryConvertToQuestionAboutAppliesToElements(model);
            TryConvertToQuestionAboutIsInherited(model);
            TryConvertToQuestionAboutPercentages(model);
            TryConvertToQuestionAboutMedia(model);
        }

        private void TryConvertToQuestionAboutValues(ImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutValues(model.Values))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatValue(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What are the possible values for the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What are the possible values for the {0} properties?", propertyName);
            }

            if (IsComplexShorthandProperty(model))
            {
                question.Text += " (shorthand property)";
            }

            question.Answers[0].Text = FormatAnswer(model.Values);

            // Create links
            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.ValuesLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "Values");
            foreach (string propertyName2 in propertyName.Split(' '))
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutValues(string answer)
        {
            return !ContainsSee(answer) || _includeAnswersThatAreReferences;
        }

        private bool IsComplexShorthandProperty(ImportModel importModel)
        {
            if (importModel.Values == null) return false;

            return importModel.Values.Contains("||") &&
                   importModel.Name != "text-decoration";
        }

        private void TryConvertToQuestionAboutInitialValue(ImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutInitialValue(model.InitialValue))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatValue(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What is the initial value of the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What are the initial values of the {0} properties?", propertyName);
            }
            question.Answers[0].Text = FormatAnswer(model.InitialValue);

            // Create links
            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.InitialValueLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "InitialValue");
            foreach (string propertyName2 in propertyName.Split(' '))
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutInitialValue(string answer)
        {
            return !ContainsSee(answer) || _includeAnswersThatAreReferences;
        }

        private void TryConvertToQuestionAboutAppliesToElements(ImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutAppliesToElements(model.AppliesTo))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatValue(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What types of elements does the {0} property apply to?", propertyName);
            }
            else
            {
                question.Text = String.Format("What types of elements do the {0} properties apply to?", propertyName);
            }

            if (String.IsNullOrWhiteSpace(model.AppliesTo))
            {
                question.Answers[0].Text = "all";
            }
            else
            {
                question.Answers[0].Text = FormatValue(model.AppliesTo);
            }

            // Create links
            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.AppliesToLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "AppliesToElements");
            foreach (string propertyName2 in propertyName.Split(' '))
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutAppliesToElements(string answer)
        {
            return !ContainsSee(answer) || _includeAnswersThatAreReferences;
        }

        private void TryConvertToQuestionAboutIsInherited(ImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutIsInherited(model.Inherited))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatValue(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("Is the {0} property inherited?", propertyName);
            }
            else
            {
                question.Text = String.Format("Are the {0} properties inherited?", propertyName);
            }
            question.Answers[0].Text = FormatAnswer(model.Inherited);

            // Create links
            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.InheritedLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "IsInherited");
            foreach (string propertyName2 in propertyName.Split(' '))
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutIsInherited(string answer)
        {
            return !ContainsSee(answer) || _includeAnswersThatAreReferences;
        }

        private void TryConvertToQuestionAboutPercentages(ImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutPercentages(model.Percentages))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatValue(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What can you say about percentage values for the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What can you say about percentage values for the {0} properties?", propertyName);
            }
            question.Answers[0].Text = FormatAnswer(model.Percentages);

            // Create links
            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.PercentagesLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "Percentages");
            foreach (string propertyName2 in propertyName.Split(' '))
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutPercentages(string answer)
        {
            return !String.IsNullOrWhiteSpace(answer) &&
                   (!ContainsSee(answer) || _includeAnswersThatAreReferences);
        }

        private void TryConvertToQuestionAboutMedia(ImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutMedia(model.Media))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatValue(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What media does the {0} property apply to?", propertyName);
            }
            else
            {
                question.Text = String.Format("What media do the {0} properties apply to?", propertyName);
            }
            question.Answers[0].Text = FormatAnswer(model.Media);

            // Create links
            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.MediaLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "Media");
            foreach (string propertyName2 in propertyName.Split(' '))
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutMedia(string answer)
        {
            return !ContainsSee(answer) || _includeAnswersThatAreReferences;
        }

        // Helpers

        private Question ConvertToQuestion_BaseMethod()
        {
            Question question = _questionRepository.Create();
            question.AutoCreateRelatedEntities(_answerRepository);
            question.SetSourceEnum(_sourceRepository, SOURCE);
            question.SetQuestionTypeEnum(_questionTypeRepository, QuestionTypeEnum.OpenQuestion);
            question.Answers[0].IsCorrectAnswer = true;
            return question;
        }

        private QuestionLink ConverToLink(LinkModel model)
        {
            QuestionLink link = _questionLinkRepository.Create();
            link.Description = model.Description;
            link.Url = model.Url;
            return link;
        }

        private void AddCategory(Question question, params string[] categoryIdentifiers)
        {
            Category category = _categoryManager.FindOrCreateCategory(categoryIdentifiers);
            QuestionCategory questionCategory = _questionCategoryRepository.Create();
            questionCategory.LinkTo(question);
            questionCategory.LinkTo(category);
        }

        private bool ContainsSee(string value)
        {
            Regex regex = new Regex(@"\bsee\b");
            return regex.IsMatch(value);
        }

        private bool IsPlural(string name)
        {
            if (name == null) return false;

            return name.Trim().Contains(" ");
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
        
        private Dictionary<string, string> _substitutions = new Dictionary<string, string>
        {
            { "<padding-width>" , "<length>, <percentage>"                                                                                      },
            { "<border-style>"  , "none, hidden, dotted, dashed, solid, double, groove, ridge, inset, outset"                                   },
            { "<border-width>"  , "thin, medium, thick, <length>"                                                                               },
            { "<shape>"         , "rect(<top>, <right>, <bottom>, <left>), rect(<length>/auto, <length>/auto, <length>/auto, <length>/auto)"    },
            { "<absolute-size>" , "xx-small, x-small, small, medium, large, x-large, xx-large"                                                  },
            { "<relative-size>" , "larger, smaller"                                                                                             },
            { "<margin-width>"  , "<length>, <percentage>, auto"                                                                                },
            { " | "             , ", "                                                                                                          },
            { " || "            , ", "                                                                                                          },
            { " ] [ "           , ", "                                                                                                          },
            { "[ "              , ""                                                                                                            },
            { " ]"              , ""                                                                                                            },
            { "["               , ""                                                                                                            },
            { "]"               , ""                                                                                                            },
            { "?"               , "(optional)"                                                                                                  },
            { "+"               , ""                                                                                                            },
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
    }
}
