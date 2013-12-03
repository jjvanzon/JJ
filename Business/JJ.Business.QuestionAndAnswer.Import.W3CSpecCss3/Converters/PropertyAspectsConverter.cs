using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JJ.Framework.Common;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Validation;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Converters
{
    public class PropertyAspectsConverter : ConverterBase<PropertyAspectsImportModel>
    {
        private bool INCLUDE_ANSWERS_THAT_ARE_REFERENCES = true;

        public PropertyAspectsConverter(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICategoryRepository categoryRepository,
            IQuestionCategoryRepository questionCategoryRepository,
            IQuestionLinkRepository questionLinkRepository,
            IQuestionTypeRepository questionTypeRepository,
            ISourceRepository sourceRepository,
            Source source,
            string categoryIdentifier)
            : base(questionRepository, answerRepository, categoryRepository, questionCategoryRepository, questionLinkRepository, questionTypeRepository, sourceRepository, source, categoryIdentifier)
        { }

        public override void ConvertToEntities(PropertyAspectsImportModel model)
        {
            TryConvertToQuestionAboutPossibleValues(model);
            TryConvertToQuestionAboutInitialValue(model);
            TryConvertToQuestionAboutAppliesToElements(model);
            TryConvertToQuestionAboutIsInherited(model);
            TryConvertToQuestionAboutPercentages(model);
            TryConvertToQuestionAboutMedia(model);
            TryConvertToQuestionAboutComputedValue(model);
            TryConvertToQuestionAboutIsAnimatable(model);
        }

        private void TryConvertToQuestionAboutPossibleValues(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutPossibleValues(model.PossibleValues))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

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

            question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.PossibleValues);

            // Create links
            if (!String.IsNullOrEmpty(model.HashTag))
            {
                QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
                hashTagLink.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.PossibleValuesLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "PossibleValues");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutPossibleValues(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            if (!INCLUDE_ANSWERS_THAT_ARE_REFERENCES && ContainsSee(answer))
            {
                return false;
            }

            return true;
        }

        private bool IsComplexShorthandProperty(PropertyAspectsImportModel importModel)
        {
            if (importModel.PossibleValues == null) return false;

            return importModel.PossibleValues.Contains("||") &&
                   importModel.PropertyName != "text-decoration"; // Special case where values syntax contains || even though it is not complex shorthand.
        }

        private void TryConvertToQuestionAboutInitialValue(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutInitialValue(model.InitialValue))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What is the initial value of the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What is the initial value for the {0} properties?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.InitialValue);

            // Create links
            if (!String.IsNullOrEmpty(model.HashTag))
            {
                QuestionLink link = CreateHashTagLink(propertyName, model.HashTag);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.InitialValueLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "InitialValue");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutInitialValue(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            if (!INCLUDE_ANSWERS_THAT_ARE_REFERENCES && ContainsSee(answer))
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutAppliesToElements(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutAppliesToElements(model.AppliesTo))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What types of elements does the {0} property apply to?", propertyName);
            }
            else
            {
                question.Text = String.Format("What types of elements do the {0} properties apply to?", FormatPluralPropertyName(propertyName));
            }

            if (String.IsNullOrWhiteSpace(model.AppliesTo))
            {
                question.Answers[0].Text = "all";
            }
            else
            {
                question.Answers[0].Text = ImportHelper.TrimValue(model.AppliesTo);
            }

            // Create links
            if (!String.IsNullOrEmpty(model.HashTag))
            {
                QuestionLink link = CreateHashTagLink(propertyName, model.HashTag);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.AppliesToLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "AppliesToElements");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutAppliesToElements(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            if (!INCLUDE_ANSWERS_THAT_ARE_REFERENCES && ContainsSee(answer))
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutIsInherited(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutIsInherited(model.IsInherited))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("Is the {0} property inherited?", propertyName);
            }
            else
            {
                question.Text = String.Format("Are the {0} properties inherited?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.IsInherited);

            // Create links
            if (!String.IsNullOrEmpty(model.HashTag))
            {
                QuestionLink link = CreateHashTagLink(propertyName, model.HashTag);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.IsInheritedLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "IsInherited");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutIsInherited(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            if (!INCLUDE_ANSWERS_THAT_ARE_REFERENCES && ContainsSee(answer))
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutPercentages(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutPercentages(model.Percentages))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What can you say about percentage values for the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What can you say about percentage values for the {0} properties?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.Percentages);

            // Create links
            if (!String.IsNullOrEmpty(model.HashTag))
            {
                QuestionLink hashTagLink = CreateHashTagLink(propertyName, model.HashTag);
                hashTagLink.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.PercentagesLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "Percentages");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutPercentages(string answer)
        {
            if (String.IsNullOrWhiteSpace(answer))
            {
                return false;
            }

            if (answer == "see individual properties")
            {
                return false;
            }

            if (answer == "N/A")
            {
                return false;
            }

            if (!INCLUDE_ANSWERS_THAT_ARE_REFERENCES && ContainsSee(answer))
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutMedia(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutMedia(model.Media))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What media does the {0} property apply to?", propertyName);
            }
            else
            {
                question.Text = String.Format("What media do the {0} properties apply to?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.Media);

            // Create links
            if (!String.IsNullOrWhiteSpace(model.HashTag))
            {
                QuestionLink link = CreateHashTagLink(propertyName, model.HashTag);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.MediaLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "Media");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutMedia(string answer)
        {
            if (answer == "see individual properties")
            {
                return false;
            }

            if (!INCLUDE_ANSWERS_THAT_ARE_REFERENCES && ContainsSee(answer))
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutComputedValue(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutComputedValue(model.ComputedValue))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("What is the computed value of the {0} property?", propertyName);
            }
            else
            {
                question.Text = String.Format("What is the computed value for the {0} properties?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.ComputedValue);

            // Create links
            if (!String.IsNullOrWhiteSpace(model.HashTag))
            {
                QuestionLink link = CreateHashTagLink(propertyName, model.HashTag);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.ComputedValueLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "ComputedValue");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutComputedValue(string answer)
        {
            if (String.IsNullOrEmpty(answer))
            {
                return false;
            }

            if (answer == "see individual properties")
            {
                return false;
            }

            if (!INCLUDE_ANSWERS_THAT_ARE_REFERENCES && ContainsSee(answer))
            {
                return false;
            }

            return true;
        }

        private void TryConvertToQuestionAboutIsAnimatable(PropertyAspectsImportModel model)
        {
            // Check conditions
            if (!MustConvertToQuestionAboutIsAnimatable(model.IsAnimatable))
            {
                return;
            }

            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string propertyName = ImportHelper.FormatTerm(model.PropertyName);

            // Set texts
            if (!IsPlural(propertyName))
            {
                question.Text = String.Format("Is the {0} property animatable?", propertyName);
            }
            else
            {
                question.Text = String.Format("Are the {0} properties animatable?", FormatPluralPropertyName(propertyName));
            }
            question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.IsAnimatable);

            // Create links
            if (!String.IsNullOrEmpty(model.HashTag))
            {
                QuestionLink link = CreateHashTagLink(propertyName, model.HashTag);
                link.LinkTo(question);
            }

            foreach (LinkModel linkModel in model.NameLinks.Union(model.IsAnimatableLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategories(question, propertyName);
            AddCategory(question, "Css3", "Properties", "Aspects", "IsAnimatable");

            // Validate result
            ValidateQuestion(question);
        }

        private bool MustConvertToQuestionAboutIsAnimatable(string isAnimatable)
        {
            return !String.IsNullOrEmpty(isAnimatable);
        }

        // Helpers

        private void AddCategories(Question question, string propertyName)
        {
            //AddCategory(question, "Css3", "Properties", "PropertyAspects");
            if (!String.IsNullOrEmpty(_categoryIdentifier))
            {
                AddCategory(question, "Css3", "Properties", _categoryIdentifier);
            }
            else
            {
                AddCategory(question, "Css3", "Properties");
            }

            foreach (string propertyName2 in ImportHelper.SplitPluralProperty(propertyName))
            {
                if (!String.IsNullOrEmpty(_categoryIdentifier))
                {
                    AddCategory(question, "Css3", "Properties", _categoryIdentifier, ImportHelper.FormatTerm(propertyName2));
                }
                else
                {
                    AddCategory(question, "Css3", "Properties", ImportHelper.FormatTerm(propertyName2));
                }
            }
        }

        private QuestionLink CreateHashTagLink(string propertyName, string hashTag)
        {
            QuestionLink link = _questionLinkRepository.Create();
            link.Description = propertyName;

            string baseUrl = _source.Url.CutRight("/");
            link.Url = baseUrl + "#" + hashTag;

            return link;
        }

        private bool ContainsSee(string value)
        {
            Regex regex = new Regex(@"\bsee\b");
            return regex.IsMatch(value);
        }

        /// <summary> Returns whether the name is plural based on whether the trimmed value contains spaces. </summary>
        private bool IsPlural(string name)
        {
            if (name == null) return false;

            return name.Trim().Contains(" ");
        }

        /*/// <summary> Removes quotes and trims. </summary>
        private string FormatPropertyName(string value)
        {
            if (value == null) return null;

            value = value.Replace("'", "");

            return value.Trim();
        }*/

        private string FormatPluralPropertyName(string value)
        {
            if (value == null) return null;

            string[] propertyNames = ImportHelper.SplitPluralProperty(value);

            return String.Join(", ", propertyNames.Take(propertyNames.Length - 1)) + " and " + propertyNames.Last();
        }
    }
}
