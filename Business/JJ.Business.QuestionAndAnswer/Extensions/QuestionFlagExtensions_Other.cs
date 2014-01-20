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
    public static class QuestionFlagExtensions_Other
    {
        public static void UnlinkRelatedEntities(this QuestionFlag questionFlag)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.Question != null)
            {
                questionFlag.LinkTo((Question)null);
            }

            if (questionFlag.FlaggedByUser != null)
            {
                questionFlag.LinkToFlaggedByUser((User)null);
            }

            if (questionFlag.FlagStatus != null)
            {
                questionFlag.LinkTo((FlagStatus)null);
            }

            if (questionFlag.LastModifiedByUser != null)
            {
                questionFlag.LinkToLastModifiedByUser((User)null);
            }
        }
    }
}
