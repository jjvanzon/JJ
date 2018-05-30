using System.Resources;

namespace JJ.Business.QuestionAndAnswer.Resources
{
    public static class ResourceFormatter
    {
        public static ResourceManager ResourceManager => Resources.ResourceManager;

        public static string AdditionalInformation => Resources.AdditionalInformation;
        public static string Answer => Resources.Answer;
        public static string AnswersCount => Resources.AnswersCount;
        public static string AreYouSureYouWishToDeleteTheFollowingQuestion => Resources.AreYouSureYouWishToDeleteTheFollowingQuestion;
        public static string AvailableCategories => Resources.AvailableCategories;
        public static string Categories => Resources.Categories;
        public static string Category => Resources.Category;
        public static string Comment => Resources.Comment;
        public static string ContentFlaggedBy => Resources.ContentFlaggedBy;
        public static string ContentFlags => Resources.ContentFlags;
        public static string ContentIsFlagged => Resources.ContentIsFlagged;
        public static string ContentManagement => Resources.ContentManagement;
        public static string ContentSourcesAndImports => Resources.ContentSourcesAndImports;
        public static string CorrectAnswerCount => Resources.CorrectAnswerCount;
        public static string CreateQuestion => Resources.CreateQuestion;
        public static string ExplanationOfIsManualWithParenthesis => Resources.ExplanationOfIsManualWithParenthesis;
        public static string FlagContent => Resources.FlagContent;
        public static string FlaggedContent => Resources.FlaggedContent;
        public static string FlagStatus => Resources.FlagStatus;
        public static string HasActiveFlags => Resources.HasActiveFlags;
        public static string HideAnswer => Resources.HideAnswer;
        public static string IsCorrectAnswer => Resources.IsCorrectAnswer;
        public static string IsManual => Resources.IsManual;
        public static string LastModifiedByUser => Resources.LastModifiedByUser;
        public static string Links => Resources.Links;
        public static string NoCategoriesAvailable => Resources.NoCategoriesAvailable;
        public static string NoQuestionsFound => Resources.NoQuestionsFound;
        public static string Question => Resources.Question;
        public static string QuestionCategory => Resources.QuestionCategory;
        public static string QuestionLink => Resources.QuestionLink;
        public static string QuestionLinks => Resources.QuestionLinks;
        public static string Questions => Resources.Questions;
        public static string QuestionType => Resources.QuestionType;
        public static string Runs => Resources.Runs;
        public static string ShowAnswer => Resources.ShowAnswer;
        public static string Source => Resources.Source;
        public static string StartTraining => Resources.StartTraining;
        public static string Statistics => Resources.Statistics;
        public static string TheCorrectAnswer => Resources.TheCorrectAnswer;
        public static string UnflagContent => Resources.UnflagContent;
        public static string YouHaveJavaScriptDisabled => Resources.YouHaveJavaScriptDisabled;

        public static string GetDisplayName(string resourceName)
        {
            string str = Resources.ResourceManager.GetString(resourceName);

            if (string.IsNullOrEmpty(str))
            {
                str = resourceName;
            }

            return str;
        }
    }
}