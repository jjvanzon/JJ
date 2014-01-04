using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionCategoryExtensions_Other
    {
        public static void UnlinkRelatedEntities(this QuestionCategory questionCategory)
        {
            if (questionCategory == null) throw new ArgumentNullException("questionCategory");

            questionCategory.Unlink(questionCategory.Question);
            questionCategory.Unlink(questionCategory.Category);
        }
    }
}
