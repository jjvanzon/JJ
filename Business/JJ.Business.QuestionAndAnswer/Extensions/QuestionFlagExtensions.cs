using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionFlagExtensions
    {
        public static void LinkTo(this QuestionFlag questionFlag, Question question)
        {
            questionFlag.Question = question;
            if (!question.QuestionFlags.Contains(questionFlag))
            {
                question.QuestionFlags.Add(questionFlag);
            }
        }

        public static void LinkToFlaggedByUser(this QuestionFlag questionFlag, User user)
        {
            questionFlag.FlaggedByUser = user;
            if (!user.AsFlaggedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsFlaggedByInQuestionFlags.Add(questionFlag);
            }
        }

        public static void LinkToLastModifiedByUser(this QuestionFlag questionFlag, User user)
        {
            questionFlag.LastModifiedByUser = user;
            if (!user.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsLastModifiedByInQuestionFlags.Add(questionFlag);
            }
        }

        public static void Unlink(this QuestionFlag questionFlag, Question question)
        {
            questionFlag.Question = null;
            if (question.QuestionFlags.Contains(questionFlag))
            {
                question.QuestionFlags.Remove(questionFlag);
            }
        }

        public static void UnlinkFlaggedByUser(this QuestionFlag questionFlag, User user)
        {
            questionFlag.FlaggedByUser = null;
            if (user.AsFlaggedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsFlaggedByInQuestionFlags.Remove(questionFlag);
            }
        }

        public static void UnlinkLastModifiedByUser(this QuestionFlag questionFlag, User user)
        {
            questionFlag.LastModifiedByUser = null;
            if (user.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsLastModifiedByInQuestionFlags.Remove(questionFlag);
            }
        }
    }
}
