using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Reflection;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class UnlinkRelatedEntitiesExtensions
    {
        public static void UnlinkRelatedEntities(this QuestionCategory questionCategory)
        {
            if (questionCategory == null) throw new NullException(() => questionCategory);

            questionCategory.UnlinkQuestion();
            questionCategory.UnlinkCategory();
        }

        /// <summary> Unlinks only the non-owned related entities. </summary>
        public static void UnlinkRelatedEntities(this Question question)
        {
            if (question == null) throw new NullException(() => question);

            question.UnlinkSource();
            question.UnlinkQuestionType();
        }

        public static void UnlinkRelatedEntities(this QuestionFlag questionFlag)
        {
            if (questionFlag == null) throw new NullException(() => questionFlag);

            questionFlag.UnlinkQuestion();
            questionFlag.UnlinkFlaggedByUser();
            questionFlag.UnlinkFlagStatus();
            questionFlag.UnlinkLastModifiedByUser();
        }

        public static void UnlinkRelatedEntities(this QuestionLink questionLink)
        {
            if (questionLink == null) throw new NullException(() => questionLink);

            questionLink.UnlinkQuestion();
        }
    }
}