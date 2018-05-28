using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Collections;
using JJ.Framework.Text;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Converters
{
	[UsedImplicitly]
	public class PropertyAspectsConverter : ConverterBase<PropertyAspectsImportModel>
	{
		private const bool INCLUDE_ANSWERS_THAT_ARE_REFERENCES = true;

		public PropertyAspectsConverter(
			IQuestionRepository questionRepository,
			IAnswerRepository answerRepository,
			ICategoryRepository categoryRepository,
			IQuestionCategoryRepository questionCategoryRepository,
			IQuestionLinkRepository questionLinkRepository,
			IQuestionTypeRepository questionTypeRepository,
			Source source,
			string categoryPath)
			: base(
				questionRepository,
				answerRepository,
				categoryRepository,
				questionCategoryRepository,
				questionLinkRepository,
				questionTypeRepository,
				source,
				categoryPath) { }

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
				question.Text = $"What are the possible values for the {propertyName} property?";
			}
			else
			{
				question.Text = $"What are the possible values for the {FormatPluralPropertyName(propertyName)} properties?";
			}

			if (IsComplexShorthandProperty(model))
			{
				question.Text += " (shorthand property)";
			}

			question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.PossibleValues);

			// Create links
			if (!string.IsNullOrEmpty(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "PossibleValues");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutPossibleValues(string answer)
		{
			if (string.Equals(answer, "see individual properties"))
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
			       importModel.PropertyName !=
			       "text-decoration"; // Special case where values syntax contains || even though it is not complex shorthand.
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
				question.Text = $"What is the initial value of the {propertyName} property?";
			}
			else
			{
				question.Text = $"What is the initial value for the {FormatPluralPropertyName(propertyName)} properties?";
			}

			question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.InitialValue);

			// Create links
			if (!string.IsNullOrEmpty(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "InitialValue");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutInitialValue(string answer)
		{
			if (string.Equals(answer, "see individual properties"))
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
				question.Text = $"What types of elements does the {propertyName} property apply to?";
			}
			else
			{
				question.Text = $"What types of elements do the {FormatPluralPropertyName(propertyName)} properties apply to?";
			}

			if (string.IsNullOrWhiteSpace(model.AppliesTo))
			{
				question.Answers[0].Text = "all";
			}
			else
			{
				question.Answers[0].Text = ImportHelper.TrimValue(model.AppliesTo);
			}

			// Create links
			if (!string.IsNullOrEmpty(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "AppliesToElements");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutAppliesToElements(string answer)
		{
			if (string.Equals(answer, "see individual properties"))
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
				question.Text = $"Is the {propertyName} property inherited?";
			}
			else
			{
				question.Text = $"Are the {FormatPluralPropertyName(propertyName)} properties inherited?";
			}

			question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.IsInherited);

			// Create links
			if (!string.IsNullOrEmpty(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "IsInherited");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutIsInherited(string answer)
		{
			if (string.Equals(answer, "see individual properties"))
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
				question.Text = $"What can you say about percentage values for the {propertyName} property?";
			}
			else
			{
				question.Text = $"What can you say about percentage values for the {FormatPluralPropertyName(propertyName)} properties?";
			}

			question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.Percentages);

			// Create links
			if (!string.IsNullOrEmpty(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "Percentages");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutPercentages(string answer)
		{
			if (string.IsNullOrWhiteSpace(answer))
			{
				return false;
			}

			if (string.Equals(answer, "see individual properties"))
			{
				return false;
			}

			if (string.Equals(answer, "N/A"))
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
				question.Text = $"What media does the {propertyName} property apply to?";
			}
			else
			{
				question.Text = $"What media do the {FormatPluralPropertyName(propertyName)} properties apply to?";
			}

			question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.Media);

			// Create links
			if (!string.IsNullOrWhiteSpace(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "Media");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutMedia(string answer)
		{
			if (string.Equals(answer, "see individual properties"))
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
				question.Text = $"What is the computed value of the {propertyName} property?";
			}
			else
			{
				question.Text = $"What is the computed value for the {FormatPluralPropertyName(propertyName)} properties?";
			}

			question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.ComputedValue);

			// Create links
			if (!string.IsNullOrWhiteSpace(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "ComputedValue");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutComputedValue(string answer)
		{
			if (string.IsNullOrEmpty(answer))
			{
				return false;
			}

			if (string.Equals(answer, "see individual properties"))
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
				question.Text = $"Is the {propertyName} property animatable?";
			}
			else
			{
				question.Text = $"Are the {FormatPluralPropertyName(propertyName)} properties animatable?";
			}

			question.Answers[0].Text = ImportHelper.ApplySubstitutionsAndTrim(model.IsAnimatable);

			// Create links
			if (!string.IsNullOrEmpty(model.HashTag))
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
			AutoCreateCategory(question, "Css3", "Properties", "Aspects", "IsAnimatable");

			// Validate result
			ValidateQuestion(question);
		}

		private bool MustConvertToQuestionAboutIsAnimatable(string isAnimatable) => !string.IsNullOrEmpty(isAnimatable);

		// Helpers

		private void AddCategories(Question question, string propertyName)
		{
			AutoCreateCategory(question, _categoryIdentifiers);

			foreach (string propertyName2 in ImportHelper.SplitPluralProperty(propertyName))
			{
				AutoCreateCategory(question, _categoryIdentifiers.Concat(ImportHelper.FormatTerm(propertyName2)));
			}
		}

		private QuestionLink CreateHashTagLink(string propertyName, string hashTag)
		{
			QuestionLink link = _questionLinkRepository.Create();
			link.Description = propertyName;

			string baseUrl = _source.Url.TrimEnd("/");
			link.Url = baseUrl + "#" + hashTag;

			return link;
		}

		private bool ContainsSee(string value)
		{
			var regex = new Regex(@"\bsee\b");
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

			return string.Join(", ", propertyNames.Take(propertyNames.Length - 1)) + " and " + propertyNames.Last();
		}
	}
}