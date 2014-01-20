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
            if (question == null) { throw new ArgumentNullException("question"); }

            if (answer.Question != null)
            {
                if (answer.Question.Answers.Contains(answer))
                {
                    answer.Question.Answers.Remove(answer);
                }
            }

            answer.Question = question;

            if (answer.Question != null)
            {
                if (!answer.Question.Answers.Contains(answer))
                {
                    answer.Question.Answers.Add(answer);
                }
            }
        }

        public static void LinkTo(this Question question, QuestionCategory questionCategory)
        {
            if (question == null) { throw new ArgumentNullException("question"); }

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

        public static void LinkTo(this Question question, QuestionLink questionLink)
        {
            if (question == null) { throw new ArgumentNullException("question"); }

            if (questionLink.Question != null)
            {
                if (questionLink.Question.QuestionLinks.Contains(questionLink))
                {
                    questionLink.Question.QuestionLinks.Remove(questionLink);
                }
            }

            questionLink.Question = question;

            if (questionLink.Question != null)
            {
                if (!questionLink.Question.QuestionLinks.Contains(questionLink))
                {
                    questionLink.Question.QuestionLinks.Add(questionLink);
                }
            }
        }

        public static void LinkTo(this Question question, QuestionType questionType)
        {
            if (question == null) { throw new ArgumentNullException("question"); }

            if (question.QuestionType != null)
            {
                if (question.QuestionType.Questions.Contains(question))
                {
                    question.QuestionType.Questions.Remove(question);
                }
            }

            question.QuestionType = questionType;

            if (question.QuestionType != null)
            {
                if (!question.QuestionType.Questions.Contains(question))
                {
                    question.QuestionType.Questions.Add(question);
                }
            }
        }

        public static void LinkTo(this Question question, Source source)
        {
            if (question == null) { throw new ArgumentNullException("question"); }

            if (question.Source != null)
            {
                if (question.Source.Questions.Contains(question))
                {
                    question.Source.Questions.Remove(question);
                }
            }

            question.Source = source;

            if (question.Source != null)
            {
                if (!question.Source.Questions.Contains(question))
                {
                    question.Source.Questions.Add(question);
                }
            }
        }
    }
}
