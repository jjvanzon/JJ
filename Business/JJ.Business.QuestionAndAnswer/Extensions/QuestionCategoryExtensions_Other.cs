using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.QuestionAndAnswer.LinkTo;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionCategoryExtensions_Other
    {
        public static void UnlinkRelatedEntities(this QuestionCategory questionCategory)
        {
            if (questionCategory == null) throw new ArgumentNullException("questionCategory");

            if (questionCategory.Question != null)
            {
                questionCategory.LinkTo((Question)null);
            }

            if (questionCategory.Category != null)
            {
                questionCategory.LinkTo((Category)null);
            }
        }
    }
}
