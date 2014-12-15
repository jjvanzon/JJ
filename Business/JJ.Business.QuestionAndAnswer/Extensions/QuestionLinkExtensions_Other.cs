using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.QuestionAndAnswer.LinkTo;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionLinkExtensions_Other
    {
        public static void UnlinkRelatedEntities(this QuestionLink questionLink)
        {
            if (questionLink == null) throw new ArgumentNullException("questionLink");

            if (questionLink.Question != null)
            {
                questionLink.LinkTo((Question)null);
            }
        }
    }
}
