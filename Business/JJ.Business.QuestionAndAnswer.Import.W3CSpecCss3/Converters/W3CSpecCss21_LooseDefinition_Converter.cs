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
    public class W3CSpecCss21_LooseDefinition_Converter : ConverterBase<W3CSpecCss21_LooseDefinition_ImportModel>
    {
        public W3CSpecCss21_LooseDefinition_Converter(
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

        public override void ConvertToEntities(W3CSpecCss21_LooseDefinition_ImportModel model)
        {
            TryConvertToQuestionFromMeaningToTerm(model);
            TryConvertToQuestionFromTermToMeaning(model);
        }

        private void TryConvertToQuestionFromMeaningToTerm(W3CSpecCss21_LooseDefinition_ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string context = FormatContextForInQuestion(model.Context);
            string term = FormatTerm(model.Term);
            string meaning = GetMeaningForInQuestion(model);
            string hashTextLinkText = TrimValue(model.HashTagLinkText);

            // Set texts
            question.Text = String.Format("In relation to {0}, what term or keyword is described as follows: {1}?", context, meaning);
            question.Answers[0].Text = term;

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(hashTextLinkText, model.HashTag);
            hashTagLink.LinkTo(question);

            foreach (LinkModel linkModel in model.ContextLinks.Union(model.TermLinks).Union(model.MeaningLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "BoxModel");
            AddCategory(question, "Css3", "Properties", "MeaningToTerm");
            AddCategory(question, "Css3", "Properties", term);
            ScanTextForExistingCategoriesAndLinkQuestionToThem(question, context);

            // Validate result
            ValidateQuestion(question);
        }

        private void TryConvertToQuestionFromTermToMeaning(W3CSpecCss21_LooseDefinition_ImportModel model)
        {
            // Create question
            Question question = ConvertToQuestion_BaseMethod();

            string context = FormatContextForInQuestion(model.Context);
            string term = FormatTerm(model.Term);
            string meaning = GetFirstSentence(model.Meaning);
            string hashTextLinkText = TrimValue(model.HashTagLinkText);

            // Set texts
            question.Text = String.Format("In relation to {0}, what does '{1}' mean?", context, term);
            question.Answers[0].Text = meaning;

            // Create links
            QuestionLink hashTagLink = CreateHashTagLink(hashTextLinkText, model.HashTag);
            hashTagLink.LinkTo(question);

            foreach (LinkModel linkModel in model.ContextLinks.Union(model.TermLinks).Union(model.MeaningLinks))
            {
                QuestionLink link = ConvertToLink(linkModel);
                link.LinkTo(question);
            }

            // Add categories
            AddCategory(question, "Css3", "Properties", "BoxModel");
            AddCategory(question, "Css3", "Properties", "TermToMeaning");
            AddCategory(question, "Css3", "Properties", term);
            ScanTextForExistingCategoriesAndLinkQuestionToThem(question, context);

            // Validate result
            ValidateQuestion(question);
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

        /// <summary>
        /// The meaning containst too many sentences. Take the first sentence,
        /// replace the term itself by a placeholder '...' so it does not give away the answer.
        /// Takes into consideration the fact that terms can be stacked up with the words 'and' and 'or'.
        /// Cut away the period at the end, so the meaning can be followed by a question mark (?).
        /// </summary>
        private string GetMeaningForInQuestion(W3CSpecCss21_LooseDefinition_ImportModel model)
        {
            string meaning = model.Meaning;

            meaning = GetFirstSentence(meaning);

            // Replace the term(s) in the meaning with '...'.
            string[] terms = model.Term.Split(new string[] { " and ", " or " });
            foreach (string term in terms)
            {
                meaning = GetMeaningForInQuestion(term, meaning);
            }

            // Cut away period from the end, so that it can be followed by a question mark (?).
            meaning = meaning.CutRight(".");

            return meaning;
        }

        /// <summary> Replaces the term itself by a placeholder '...' so that the definition can be used in a question without giving away the answer. </summary>
        private string GetMeaningForInQuestion(string term, string meaning)
        {
            meaning = TrimValue(meaning);
            meaning = meaning.Replace("The " + term, "...");
            meaning = meaning.Replace(" the " + term, " ...");
            meaning = meaning.Replace("A " + term, "...");
            meaning = meaning.Replace(" a " + term, " ...");
            meaning = meaning.Replace(term, "...");
            return meaning;
        }

        /// <summary> Trims and cuts off &lt; from the beginning and &gt; from the end. </summary>
        private string FormatTerm(string value)
        {
            value = TrimValue(value);
            value = value.CutLeft("<");
            value = value.CutRight(">");
            return value;
        }

        /// <summary> De-capitalizes the first letter. </summary>
        private string FormatContextForInQuestion(string context)
        {
            if (String.IsNullOrEmpty(context))
            {
                return context;
            }

            // Decapitalize
            context = context.Left(1).ToLower() + context.CutLeft(1);

            return context;
        }

        /// <summary> Trims, but does not throw exception when value is null. </summary>
        private string TrimValue(string value)
        {
            if (String.IsNullOrEmpty(value)) 
            {
                return value;
            }

            return value.Trim();
        }

        /// <summary> Gets the text up until the first period (.) or until the end of the string. </summary>
        private string GetFirstSentence(string value)
        {
            var regex = new Regex(@"^[^\.$]*(\.|$)"); // Start of string, any character that is not period (.) or end of string, followed by a period (.) or end of string.
            Match match = regex.Match(value);

            if (match == null)
            {
                throw new Exception(String.Format("Error trying to extract the first sentence of the following text: '{0}'.", value));
            }

            return match.Value;
        }

        private void ScanTextForExistingCategoriesAndLinkQuestionToThem(Question question, string context)
        {
            // TODO: This is pointless if the source of information renders only terrible questions. Perhaps try this solution in a later import, where it is worth is.

            // E.g. "Margin properties: 'margin-top', 'margin-right', 'margin-bottom', 'margin-left', and 'margin'"

            // Split words
            // Go by words, and n numer of next words.
            // Find category "Css3" "Properties" "<words>"
            // If it exist, link question to category.
            // Possible create some extra categories before processing the file.
            // TODO: Also do this trick in other imports.

            // If you are going to add a category Margin Properties. other imports should also add that category.
            /*AddCategory(question, "Css3", "Properties", "Margin Properties");

            // This is not good
            AddCategory(question, "Css3", "Properties", "Margin");
            AddCategory(question, "Css3", "Properties", "Properties");

            AddCategory(question, "Css3", "Properties", "margin-top");
            AddCategory(question, "Css3", "Properties", "margin-right");
            // This is not good:
            AddCategory(question, "Css3", "Properties", "and");

            // What if you go through all words or consecutive strings of words,
            // Look up if the category exists, and if so, add the question to it.
            // It is not full proof, in various ways: it might create false matches,
            // and also this import will become dependent on the results of previous imports.
            throw new NotImplementedException();*/
        }
    }
}
