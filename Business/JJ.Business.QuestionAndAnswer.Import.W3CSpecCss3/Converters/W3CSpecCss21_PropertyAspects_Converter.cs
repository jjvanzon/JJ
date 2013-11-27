using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JJ.Business.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Framework.Validation;
using JJ.Framework.Common;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Converters
{
    public class W3CSpecCss21_PropertyAspects_Converter : ConverterBase<W3CSpecCss21_PropertyAspects_ImportModel>
    {
        public W3CSpecCss21_PropertyAspects_Converter(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionTypeRepository questionTypeRepository,
            ISourceRepository sourceRepository,
            Source source)
            : base(questionRepository, answerRepository, categoryRepository, questionCategoryRepository, questionLinkRepository, questionTypeRepository, sourceRepository, source)
        { }

        public override void ConvertToEntities(W3CSpecCss21_PropertyAspects_ImportModel model)
        {
            TryConvertToQuestionAboutValues(model);
            TryConvertToQuestionAboutInitialValue(model);
            TryConvertToQuestionAboutAppliesToElements(model);
            TryConvertToQuestionAboutIsInherited(model);
            TryConvertToQuestionAboutPercentages(model);
            TryConvertToQuestionAboutMedia(model);
            TryConvertToQuestionAboutComputedValue(model);
        }

        private void TryConvertToQuestionAboutValues(W3CSpecCss21_PropertyAspects_ImportModel model)
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

            foreach (LinkModel linkModel in model.NameLinks.Union(model.ValueLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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

        private bool IsComplexShorthandProperty(W3CSpecCss21_PropertyAspects_ImportModel importModel)
        {
            if (importModel.Value == null) return false;

            return importModel.Value.Contains("||");

            // This one might be relevant when you use this same code in other imports.
            /*return importModel.Value.Contains("||") &&
                   importModel.Name != "text-decoration";*/
        }

        private void TryConvertToQuestionAboutInitialValue(W3CSpecCss21_PropertyAspects_ImportModel model)
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

            foreach (LinkModel linkModel in model.NameLinks.Union(model.InitialLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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

        private void TryConvertToQuestionAboutAppliesToElements(W3CSpecCss21_PropertyAspects_ImportModel model)
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

            foreach (LinkModel linkModel in model.NameLinks.Union(model.AppliesToLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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

        private void TryConvertToQuestionAboutIsInherited(W3CSpecCss21_PropertyAspects_ImportModel model)
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

            foreach (LinkModel linkModel in model.NameLinks.Union(model.InheritedLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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

        private void TryConvertToQuestionAboutPercentages(W3CSpecCss21_PropertyAspects_ImportModel model)
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

            foreach (LinkModel linkModel in model.NameLinks.Union(model.PercentagesLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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

        private void TryConvertToQuestionAboutMedia(W3CSpecCss21_PropertyAspects_ImportModel model)
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

            foreach (LinkModel linkModel in model.NameLinks.Union(model.MediaLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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

        private void TryConvertToQuestionAboutComputedValue(W3CSpecCss21_PropertyAspects_ImportModel model)
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

            foreach (LinkModel linkModel in model.NameLinks.Union(model.ComputedValueLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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

        private QuestionLink CreateHashTagLink(string propertyName, string hashTag)
        {
            QuestionLink link = _questionLinkRepository.Create();
            link.Description = propertyName;

            string baseUrl = _source.Url.CutRight("/");
            link.Url = baseUrl + "#" + hashTag;

            return link;
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
