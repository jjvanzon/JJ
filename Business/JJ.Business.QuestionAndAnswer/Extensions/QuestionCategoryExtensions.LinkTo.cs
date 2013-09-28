using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static partial class QuestionCategoryExtensions
    {
        public static void LinkTo(this QuestionCategory questionCategory, Question question)
        {
            if (questionCategory == null)
            {
                throw new ArgumentNullException("questionCategory");
            }
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }

            questionCategory.Question = question;
            question.QuestionCategories.Add(questionCategory);
        }
    }
}
