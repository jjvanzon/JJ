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

            if (questionFlag.Question != null)
            {
                if (questionFlag.Question.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.Question.QuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.Question = question;

            if (questionFlag.Question != null)
            {
                if (!questionFlag.Question.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.Question.QuestionFlags.Add(questionFlag);
                }
            }
        }

        public static void LinkTo(this QuestionFlag questionFlag, FlagStatus flagStatus)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.FlagStatus != null)
            {
                if (questionFlag.FlagStatus.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlagStatus.QuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.FlagStatus = flagStatus;

            if (questionFlag.FlagStatus != null)
            {
                if (!questionFlag.FlagStatus.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlagStatus.QuestionFlags.Add(questionFlag);
                }
            }
        }

        public static void LinkToFlaggedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.FlaggedByUser != null)
            {
                if (questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.FlaggedByUser = user;

            if (questionFlag.FlaggedByUser != null)
            {
                if (!questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Add(questionFlag);
                }
            }
        }

        public static void LinkToLastModifiedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.LastModifiedByUser != null)
            {
                if (questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.LastModifiedByUser = user;

            if (questionFlag.LastModifiedByUser != null)
            {
                if (!questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Add(questionFlag);
                }
            }
        }

        //public static void Unlink(this QuestionFlag questionFlag, Question question)
        //{
        //    if (questionFlag == null) throw new ArgumentNullException("questionFlag");
        //    if (question == null) throw new ArgumentNullException("question");

        //    questionFlag.Unlink((Question)null);
        //}

        //public static void Unlink(this QuestionFlag questionFlag, FlagStatus flagStatus)
        //{
        //    if (questionFlag == null) throw new ArgumentNullException("questionFlag");
        //    if (flagStatus == null) throw new ArgumentNullException("flagStatus");

        //    questionFlag.LinkTo((FlagStatus)null);
        //}

        //public static void UnlinkFlaggedByUser(this QuestionFlag questionFlag, User user)
        //{
        //    if (questionFlag == null) throw new ArgumentNullException("questionFlag");
        //    if (user == null) throw new ArgumentNullException("user");

        //    questionFlag.LinkToFlaggedByUser((User)null);
        //}

        //public static void UnlinkLastModifiedByUser(this QuestionFlag questionFlag, User user)
        //{
        //    if (questionFlag == null) throw new ArgumentNullException("questionFlag");
        //    if (user == null) throw new ArgumentNullException("user");

        //    questionFlag.LinkToLastModifiedByUser((User)null);
        //}
    }
}
