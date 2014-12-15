using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.LinkTo
{
    public static class QuestionCategoryExtensions
    {
        public static void LinkTo(this QuestionCategory questionCategory, Question question)
        {
            if (questionCategory == null) { throw new ArgumentNullException("questionCategory"); }

            if (questionCategory.Question != null)
            {
                if (questionCategory.Question.QuestionCategories.Contains(questionCategory))
                {
                    questionCategory.Question.QuestionCategories.Remove(questionCategory);
                }
            }

            questionCategory.Question = question;

            if (questionCategory.Question != null)
            {
                if (!questionCategory.Question.QuestionCategories.Contains(questionCategory))
                {
                    questionCategory.Question.QuestionCategories.Add(questionCategory);
                }
            }
        }

        public static void LinkTo(this QuestionCategory questionCategory, Category category)
        {
            if (questionCategory == null) { throw new ArgumentNullException("questionCategory"); }

            if (questionCategory.Category != null)
            {
                if (questionCategory.Category.CategoryQuestions.Contains(questionCategory))
                {
                    questionCategory.Category.CategoryQuestions.Remove(questionCategory);
                }
            }

            questionCategory.Category = category;

            if (questionCategory.Category != null)
            {
                if (!questionCategory.Category.CategoryQuestions.Contains(questionCategory))
                {
                    questionCategory.Category.CategoryQuestions.Add(questionCategory);
                }
            }
        }

        //public static void Unlink(this QuestionCategory questionCategory, Question question)
        //{
        //    if (questionCategory == null) throw new ArgumentNullException("questionCategory");
        //    if (question == null) throw new ArgumentNullException("question");

        //    questionCategory.LinkTo((Question)null);
        //}

        //public static void Unlink(this QuestionCategory questionCategory, Category category)
        //{
        //    if (questionCategory == null) throw new ArgumentNullException("questionCategory");
        //    if (category == null) throw new ArgumentNullException("category");

        //    questionCategory.LinkTo((Category)null);
        //}
    }
}
