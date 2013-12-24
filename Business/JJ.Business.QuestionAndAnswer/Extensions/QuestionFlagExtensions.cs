using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionFlagExtensions
    {
        public static void UnlinkRelatedEntities(this QuestionFlag questionFlag)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.Question != null)
            {
                questionFlag.Unlink(questionFlag.Question);
            }

            if (questionFlag.FlaggedByUser != null)
            {
                questionFlag.UnlinkFlaggedByUser(questionFlag.FlaggedByUser);
            }

            if (questionFlag.FlagStatus != null)
            {
                questionFlag.Unlink(questionFlag.FlagStatus);
            }

            if (questionFlag.LastModifiedByUser != null)
            {
                questionFlag.UnlinkLastModifiedByUser(questionFlag.LastModifiedByUser);
            }
        }
    }
}
