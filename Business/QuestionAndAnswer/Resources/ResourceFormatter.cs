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
        public static string BackToList => Resources.BackToList;
        public static string Categories => Resources.Categories;
        public static string Category => Resources.Category;
        public static string Comment => Resources.Comment;
        public static string Confirm => Resources.Confirm;
        public static string ContentFlaggedBy => Resources.ContentFlaggedBy;
        public static string ContentFlags => Resources.ContentFlags;
        public static string ContentIsFlagged => Resources.ContentIsFlagged;
        public static string ContentManagement => Resources.ContentManagement;
        public static string ContentSourcesAndImports => Resources.ContentSourcesAndImports;
        public static string CorrectAnswerCount => Resources.CorrectAnswerCount;
        public static string CreateQuestion => Resources.CreateQuestion;
        public static string Date => Resources.Date;
        public static string DateTime => Resources.DateTime;
        public static string DeleteQuestion => Resources.DeleteQuestion;
        public static string EditQuestion => Resources.EditQuestion;
        public static string ExplanationOfIsManualWithParenthesis => Resources.ExplanationOfIsManualWithParenthesis;
        public static string FlagContent => Resources.FlagContent;
        public static string FlaggedContent => Resources.FlaggedContent;
        public static string FlagStatus => Resources.FlagStatus;
        public static string General => Resources.General;
        public static string HasActiveFlags => Resources.HasActiveFlags;
        public static string HideAnswer => Resources.HideAnswer;
        public static string ID => Resources.ID;
        public static string IsActive => Resources.IsActive;
        public static string IsCorrectAnswer => Resources.IsCorrectAnswer;
        public static string IsManual => Resources.IsManual;
        public static string LastModifiedByUser => Resources.LastModifiedByUser;
        public static string Links => Resources.Links;
        public static string NextQuestion => Resources.NextQuestion;
        public static string NoCategoriesAvailable => Resources.NoCategoriesAvailable;
        public static string NoQuestionsFound => Resources.NoQuestionsFound;
        public static string NotAuthorizedMessage => Resources.NotAuthorizedMessage;
        public static string Question => Resources.Question;
        public static string QuestionCategory => Resources.QuestionCategory;
        public static string QuestionDetails => Resources.QuestionDetails;
        public static string QuestionIsDeleted => Resources.QuestionIsDeleted;
        public static string QuestionLink => Resources.QuestionLink;
        public static string QuestionLinks => Resources.QuestionLinks;
        public static string Questions => Resources.Questions;
        public static string QuestionType => Resources.QuestionType;
        public static string Runs => Resources.Runs;
        public static string SelectCategories => Resources.SelectCategories;
        public static string Selection => Resources.Selection;
        public static string ShowAnswer => Resources.ShowAnswer;
        public static string Source => Resources.Source;
        public static string StartTraining => Resources.StartTraining;
        public static string Statistics => Resources.Statistics;
        public static string Text => Resources.Text;
        public static string TheCorrectAnswer => Resources.TheCorrectAnswer;
        public static string Type => Resources.Type;
        public static string UnflagContent => Resources.UnflagContent;
        public static string Url => Resources.Url;
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