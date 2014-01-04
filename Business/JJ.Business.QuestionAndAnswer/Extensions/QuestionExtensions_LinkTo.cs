using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionExtensions_LinkTo
    {
        public static void LinkTo(this Question question, Answer answer)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }
            if (answer == null)
            {
                throw new ArgumentNullException("answer");
            }

            question.Answers.Add(answer);
            answer.Question = question;
        }

        public static void LinkTo(this Question question, QuestionCategory questionCategory)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }
            if (questionCategory == null)
            {
                throw new ArgumentNullException("questionCategory");
            }

            question.QuestionCategories.Add(questionCategory);
            questionCategory.Question = question;
        }

        public static void LinkTo(this Question question, QuestionLink questionLink)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }
            if (questionLink == null)
            {
                throw new ArgumentNullException("questionLink");
            }

            question.QuestionLinks.Add(questionLink);
            questionLink.Question = question;
        }

        public static void LinkTo(this Question question, QuestionType questionType)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }
            if (questionType == null)
            {
                throw new ArgumentNullException("questionLink");
            }

            question.QuestionType = questionType;
            questionType.Questions.Add(question);
        }

        public static void LinkTo(this Question question, Source source)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            question.Source = source;
            source.Questions.Add(question);
        }
    }
}
