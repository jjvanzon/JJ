using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer;
using JJ.Framework.Validation;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Converters
{
    public class W3CSpecCss3_PropertyIndex_Converter : ConverterBase<W3CSpecCss3_PropertyIndex_ImportModel>
    {
        public W3CSpecCss3_PropertyIndex_Converter(
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

        private bool INCLUDE_ANSWERS_THAT_ARE_REFERENCES = true;

        public override void ConvertToEntities(W3CSpecCss3_PropertyIndex_ImportModel model)
        {
            TryConvertToQuestionAboutValues(model);
            TryConvertToQuestionAboutInitialValue(model);
            TryConvertToQuestionAboutAppliesToElements(model);
            TryConvertToQuestionAboutIsInherited(model);
            TryConvertToQuestionAboutPercentages(model);
            TryConvertToQuestionAboutMedia(model);
        }

        private void TryConvertToQuestionAboutValues(W3CSpecCss3_PropertyIndex_ImportModel model)
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
            foreach (LinkModel linkModel in model.NameLinks.Union(model.ValuesLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "PossibleValues");
            foreach (string propertyName2 in propertyName.Split(' '))
            {
                AddCategory(question, "Css3", "Properties", propertyName2);
            }

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutValues(string answer)
        {
            return !ContainsSee(answer) || INCLUDE_ANSWERS_THAT_ARE_REFERENCES;
        }

        private bool IsComplexShorthandProperty(W3CSpecCss3_PropertyIndex_ImportModel importModel)
        {
            if (importModel.Values == null) return false;

            return importModel.Values.Contains("||") &&
                   importModel.Name != "text-decoration";
        }

        private void TryConvertToQuestionAboutInitialValue(W3CSpecCss3_PropertyIndex_ImportModel model)
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
            foreach (LinkModel linkModel in model.NameLinks.Union(model.InitialValueLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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
            return !ContainsSee(answer) || INCLUDE_ANSWERS_THAT_ARE_REFERENCES;
        }

        private void TryConvertToQuestionAboutAppliesToElements(W3CSpecCss3_PropertyIndex_ImportModel model)
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
            foreach (LinkModel linkModel in model.NameLinks.Union(model.AppliesToLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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
            return !ContainsSee(answer) || INCLUDE_ANSWERS_THAT_ARE_REFERENCES;
        }

        private void TryConvertToQuestionAboutIsInherited(W3CSpecCss3_PropertyIndex_ImportModel model)
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
            foreach (LinkModel linkModel in model.NameLinks.Union(model.InheritedLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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
            return !ContainsSee(answer) || INCLUDE_ANSWERS_THAT_ARE_REFERENCES;
        }

        private void TryConvertToQuestionAboutPercentages(W3CSpecCss3_PropertyIndex_ImportModel model)
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
            foreach (LinkModel linkModel in model.NameLinks.Union(model.PercentagesLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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
                   (!ContainsSee(answer) || INCLUDE_ANSWERS_THAT_ARE_REFERENCES);
        }

        private void TryConvertToQuestionAboutMedia(W3CSpecCss3_PropertyIndex_ImportModel model)
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
            foreach (LinkModel linkModel in model.NameLinks.Union(model.MediaLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
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
            return !ContainsSee(answer) || INCLUDE_ANSWERS_THAT_ARE_REFERENCES;
        }

        // Helpers

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
