using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.LinkTo
{
    public static class UnlinkExtensions
    {
        public static void UnlinkSource(this Question question)
        {
            question.LinkTo((Source)null);
        }

        public static void UnlinkQuestionType(this Question question)
        {
            question.LinkTo((QuestionType)null);
        }

        public static void UnlinkQuestion(this QuestionCategory questionCategory)
        {
            questionCategory.LinkTo((Question)null);
        }

        public static void UnlinkCategory(this QuestionCategory questionCategory)
        {
            questionCategory.LinkTo((Category)null);
        }

        public static void UnlinkQuestion(this QuestionFlag questionFlag)
        {
            questionFlag.LinkTo((Question)null);
        }

        public static void UnlinkFlagStatus(this QuestionFlag questionFlag)
        {
            questionFlag.LinkTo((FlagStatus)null);
        }

        public static void UnlinkFlaggedByUser(this QuestionFlag questionFlag)
        {
            questionFlag.LinkToFlaggedByUser((User)null);
        }

        public static void UnlinkLastModifiedByUser(this QuestionFlag questionFlag)
        {
            questionFlag.LinkToLastModifiedByUser((User)null);
        }

        public static void UnlinkQuestion(this QuestionLink questionLink)
        {
            questionLink.LinkTo((Question)null);
        }

        public static void UnlinkQuestion(this Answer answer)
        {
            answer.LinkTo((Question)null);
        }
    }
}
