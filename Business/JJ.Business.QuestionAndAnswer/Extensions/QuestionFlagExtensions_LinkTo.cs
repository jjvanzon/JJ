using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionFlagExtensions_LinkTo
    {
        public static void LinkTo(this QuestionFlag questionFlag, Question question)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (question == null) throw new ArgumentNullException("question");

            questionFlag.Question = question;
            if (!question.QuestionFlags.Contains(questionFlag))
            {
                question.QuestionFlags.Add(questionFlag);
            }
        }

        public static void LinkTo(this QuestionFlag questionFlag, FlagStatus flagStatus)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (flagStatus == null) throw new ArgumentNullException("flagStatus");

            questionFlag.FlagStatus = flagStatus;
            if (!flagStatus.QuestionFlags.Contains(questionFlag))
            {
                flagStatus.QuestionFlags.Add(questionFlag);
            }
        }

        public static void LinkToFlaggedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (user == null) throw new ArgumentNullException("user");

            questionFlag.FlaggedByUser = user;
            if (!user.AsFlaggedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsFlaggedByInQuestionFlags.Add(questionFlag);
            }
        }

        public static void LinkToLastModifiedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (user == null) throw new ArgumentNullException("user");

            questionFlag.LastModifiedByUser = user;
            if (!user.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsLastModifiedByInQuestionFlags.Add(questionFlag);
            }
        }

        public static void Unlink(this QuestionFlag questionFlag, Question question)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (question == null) throw new ArgumentNullException("question");

            questionFlag.Question = null;
            if (question.QuestionFlags.Contains(questionFlag))
            {
                question.QuestionFlags.Remove(questionFlag);
            }
        }

        public static void Unlink(this QuestionFlag questionFlag, FlagStatus flagStatus)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (flagStatus == null) throw new ArgumentNullException("flagStatus");

            questionFlag.FlagStatus = null;
            if (flagStatus.QuestionFlags.Contains(questionFlag))
            {
                flagStatus.QuestionFlags.Remove(questionFlag);
            }
        }

        public static void UnlinkFlaggedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (user == null) throw new ArgumentNullException("user");

            questionFlag.FlaggedByUser = null;
            if (user.AsFlaggedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsFlaggedByInQuestionFlags.Remove(questionFlag);
            }
        }

        public static void UnlinkLastModifiedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");
            if (user == null) throw new ArgumentNullException("user");

            questionFlag.LastModifiedByUser = null;
            if (user.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
            {
                user.AsLastModifiedByInQuestionFlags.Remove(questionFlag);
            }
        }
    }
}
