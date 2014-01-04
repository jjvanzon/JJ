using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionCategoryExtensions_LinkTo
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

        public static void LinkTo(this QuestionCategory questionCategory, Category category)
        {
            if (questionCategory == null)
            {
                throw new ArgumentNullException("questionCategory");
            }
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }

            questionCategory.Category = category;
            category.CategoryQuestions.Add(questionCategory);
        }

        public static void Unlink(this QuestionCategory questionCategory, Question question)
        {
            if (questionCategory == null) throw new ArgumentNullException("questionCategory");
            if (question == null) throw new ArgumentNullException("question");

            questionCategory.Question = null;
            if (question.QuestionCategories.Contains(questionCategory))
            {
                question.QuestionCategories.Remove(questionCategory);
            }
        }

        public static void Unlink(this QuestionCategory questionCategory, Category category)
        {
            if (questionCategory == null) throw new ArgumentNullException("questionCategory");
            if (category == null) throw new ArgumentNullException("category");

            questionCategory.Category = null;
            if (category.CategoryQuestions.Contains(questionCategory))
            {
                category.CategoryQuestions.Remove(questionCategory);
            }
        }
    }
}
