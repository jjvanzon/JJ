using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JJ.Framework.Common;
using JJ.Framework.Validation;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Business.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Models;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Processing
{
    public class PropertyDefinitionConverter
    {
        private const SourceEnum SOURCE = SourceEnum.W3CSpecCss21BoxModel;

        private IQuestionRepository _questionRepository;
        private IAnswerRepository _answerRepository;
        private ICategoryRepository _categoryRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IQuestionLinkRepository _questionLinkRepository;
        private IQuestionTypeRepository _questionTypeRepository;
        private ISourceRepository _sourceRepository;

        private CategoryManager _categoryManager;

        public PropertyDefinitionConverter(
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

            _categoryManager = new CategoryManager(_categoryRepository);
        }

        public void ConvertToEntities(PropertyDefinitionModel model)
        {
            TryConvertToQuestionAboutValues(model);
            TryConvertToQuestionAboutInitialValue(model);
            TryConvertToQuestionAboutAppliesToElements(model);
            TryConvertToQuestionAboutIsInherited(model);
            TryConvertToQuestionAboutPercentages(model);
            TryConvertToQuestionAboutMedia(model);
            TryConvertToQuestionAboutComputedValue(model);
        }

        private void TryConvertToQuestionAboutValues(PropertyDefinitionModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutValues(model.Value))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatPropertyName(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What are the possible values for the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What are the possible values for the {0} properties?", FormatPluralPropertyName(propertyName));
            }

            if (IsComplexShorthandProperty(model))
            {
                question.Text += " (shorthand property)";
            }

            question.Answers[0].Text = FormatAnswer(model.Value);

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
            hashTagLink.LinkTo(question);

            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.ValueLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "PossibleValues");
            AddCategory(question, "Css3", "Properties", "BoxModel");
            foreach (string propertyName2 in propertyName.Split(',').TrimAll())
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutValues(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            return true;
        }

        private bool IsComplexShorthandProperty(PropertyDefinitionModel importModel)
        {
            if (importModel.Value == null) return false;

            return importModel.Value.Contains("||");

            // This one might be relevant when you use this same code in other imports.
            /*return importModel.Value.Contains("||") &&
                   importModel.Name != "text-decoration";*/
        }

        private void TryConvertToQuestionAboutInitialValue(PropertyDefinitionModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutInitialValue(model.Initial))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatPropertyName(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What is the initial value of the {0} property?", propertyName);
            }
            else
            {
                // TODO: Use same phrase in other import.
                question.Text = String.Format("What is the initial value for the {0} properties?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = FormatAnswer(model.Initial);

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
            hashTagLink.LinkTo(question);

            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.InitialLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "InitialValue");
            AddCategory(question, "Css3", "Properties", "BoxModel");
            foreach (string propertyName2 in propertyName.Split(',').TrimAll())
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutInitialValue(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutAppliesToElements(PropertyDefinitionModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutAppliesToElements(model.AppliesTo))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatPropertyName(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What types of elements does the {0} property apply to?", propertyName);
            }
            else
            {
                question.Text = String.Format("What types of elements do the {0} properties apply to?", FormatPluralPropertyName(propertyName));
            }

            question.Answers[0].Text = FormatValue(model.AppliesTo);

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
            hashTagLink.LinkTo(question);

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
            AddCategory(question, "Css3", "Properties", "BoxModel");
            foreach (string propertyName2 in propertyName.Split(',').TrimAll())
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutAppliesToElements(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutIsInherited(PropertyDefinitionModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutIsInherited(model.Inherited))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatPropertyName(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("Is the {0} property inherited?", propertyName);
            }
            else
            {
                question.Text = String.Format("Are the {0} properties inherited?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = FormatAnswer(model.Inherited);

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
            hashTagLink.LinkTo(question);

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
            AddCategory(question, "Css3", "Properties", "BoxModel");
            foreach (string propertyName2 in propertyName.Split(',').TrimAll())
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutIsInherited(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutPercentages(PropertyDefinitionModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutPercentages(model.Percentages))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatPropertyName(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What can you say about percentage values for the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What can you say about percentage values for the {0} properties?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = FormatAnswer(model.Percentages);

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
            hashTagLink.LinkTo(question);

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
            AddCategory(question, "Css3", "Properties", "BoxModel");
            foreach (string propertyName2 in propertyName.Split(',').TrimAll())
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutPercentages(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            if (answer == "N/A")
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutMedia(PropertyDefinitionModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutMedia(model.Media))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatPropertyName(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What media does the {0} property apply to?", propertyName);
            }
            else
            {
                question.Text = String.Format("What media do the {0} properties apply to?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = FormatAnswer(model.Media);

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
            hashTagLink.LinkTo(question);

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
            AddCategory(question, "Css3", "Properties", "BoxModel");
            foreach (string propertyName2 in propertyName.Split(',').TrimAll())
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutMedia(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutComputedValue(PropertyDefinitionModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutComputedValue(model.ComputedValue))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = FormatPropertyName(model.Name);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What is the computed value of the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What is the computed value for the {0} properties?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = FormatAnswer(model.ComputedValue);

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
            hashTagLink.LinkTo(question);

            foreach (LinkModel linkModel in model.NameLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.ComputedValueLinks)
            {
                QuestionLink link = ConverToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "ComputedValue");
            AddCategory(question, "Css3", "Properties", "BoxModel");
            foreach (string propertyName2 in propertyName.Split(',').TrimAll())
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutComputedValue(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            return true;
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

            // Some terms have < > around them, and sometimes even '< and >'. Those are ugly in a link.
            if (link.Description != null)
            {
                link.Description = link.Description.Replace("<'", "")
                                                   .Replace("'>", "")
                                                   .Replace(">", "")
                                                   .Replace("<", "");
            }

            return link;
        }

        private QuestionLink CreateHashTagLink(string propertyName, string hashTag)
        {
            QuestionLink link = _questionLinkRepository.Create();
            link.Description = propertyName;

            Source source = _sourceRepository.Get((int)SOURCE);
            string baseUrl = source.Link.CutRight("/");
            link.Url = baseUrl + "#" + hashTag;

            return link;
        }

        private void AddCategory(Question question, params string[] categoryIdentifiers)
        {
            Category category = _categoryManager.FindOrCreateCategoryByIdentifierPath(categoryIdentifiers);
            QuestionCategory questionCategory = _questionCategoryRepository.Create();
            questionCategory.LinkTo(question);
            questionCategory.LinkTo(category);
        }

        private bool IsPlural(string name)
        {
            if (name == null) return false;

            return name.Trim().Contains(" ");
        }

        private string FormatPropertyName(string value)
        {
            if (value == null) return null;

            value = value.Replace("'", "");

            return value.Trim();
        }

        // TODO: also do this in ImportW3CSpecCss3PropertyIndex.
        private string FormatPluralPropertyName(string value)
        {
            if (value == null) return null;

            string[] propertyNames = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).TrimAll();

            return String.Join(", ", propertyNames.Take(propertyNames.Length - 1)) + " and " + propertyNames.Last();
        }

        private string FormatValue(string value)
        {
            if (value == null) return null;

            return value.Trim();
        }

        private void ValidateQuestion(Question question)
        {
            IValidator validator = new QuestionOpenQuestionValidator(question);
            validator.Verify();
        }

        private Dictionary<string, string> _substitutionsInAnswer = new Dictionary<string, string>
        {
            { " | "                , ", "                                                                                                       },
            { " || "               , ", "                                                                                                       },
            { " ] [ "              , ", "                                                                                                       },
            { "[ "                 , ""                                                                                                         },
            { " ]"                 , ""                                                                                                         },
            { "["                  , ""                                                                                                         },
            { "]"                  , ""                                                                                                         },
            { "?"                  , "(optional)"                                                                                               },
            { "+"                  , ""                                                                                                         },
            { "<'"                 , "<"                                                                                                        },
            { "'>"                 , ">"                                                                                                        },
            { "<padding-width>"    , "<length>, <percentage>"                                                                                   },
            { "<border-style>"     , "none, hidden, dotted, dashed, solid, double, groove, ridge, inset, outset"                                },
            { "<border-width>"     , "thin, medium, thick, <length>"                                                                            },
            { "<shape>"            , "rect(<top>, <right>, <bottom>, <left>), rect(<length>/auto, <length>/auto, <length>/auto, <length>/auto)" },
            { "<absolute-size>"    , "xx-small, x-small, small, medium, large, x-large, xx-large"                                               },
            { "<relative-size>"    , "larger, smaller"                                                                                          },
            { "<margin-width>"     , "<length>, <percentage>, auto"                                                                             },
            { "<border-top-color>" , "<color>, transparent, inherit"                                                                            },
            { "inherit, inherit"   , "inherit"                                                                                                  }
        };

        private string FormatAnswer(string input)
        {
            if (input == null)
            {
                return null;
            }

            string value = input;

            foreach (var x in _substitutionsInAnswer)
            {
                value = value.Replace(x.Key, x.Value);
            }

            value = value.Trim();

            return value;
        }
    }
}
