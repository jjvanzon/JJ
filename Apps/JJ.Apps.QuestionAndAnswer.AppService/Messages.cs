using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.AppService
{
    public class Messages
    {
        public string AreYouSureYouWishToDeleteTheFollowingQuestion { get; set; }
        public string ExplanationOfIsManualWithParenthesis { get; set; }
        public string NoCategoriesAvailable { get; set; }
        public string NoQuestionsFound { get; set; }
        public string QuestionIsDeleted { get; set; }
    }
}