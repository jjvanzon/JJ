using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.LinkTo
{
    public static class LinkToExtensions
    {
        public static void LinkTo(this Answer answer, Question question)
        {
            if (answer == null) { throw new ArgumentNullException("answer"); }

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

        public static void LinkToParentCategory(this Category category, Category parentCategory)
        {
            if (category == null) { throw new ArgumentNullException("category"); }

            if (category.ParentCategory != null)
            {
                if (parentCategory.SubCategories.Contains(category))
                {
                    parentCategory.SubCategories.Remove(category);
                }
            }

            category.ParentCategory = parentCategory;

            if (category.ParentCategory != null)
            {
                if (!parentCategory.SubCategories.Contains(category))
                {
                    parentCategory.SubCategories.Add(category);
                }
            }
        }

        public static void LinkToSubCategory(this Category category, Category subCategory)
        {
            if (category == null) { throw new ArgumentNullException("category"); }

            if (subCategory.ParentCategory.SubCategories.Contains(subCategory))
            {
                subCategory.ParentCategory.SubCategories.Remove(subCategory);
            }

            subCategory.ParentCategory = category;

            if (!subCategory.ParentCategory.SubCategories.Contains(subCategory))
            {
                subCategory.ParentCategory.SubCategories.Add(subCategory);
            }
        }

        public static void LinkTo(this Category category, QuestionCategory categoryQuestion)
        {
            if (category == null) { throw new ArgumentNullException("category"); }

            if (categoryQuestion.Category != null)
            {
                if (categoryQuestion.Category.CategoryQuestions.Contains(categoryQuestion))
                {
                    categoryQuestion.Category.CategoryQuestions.Remove(categoryQuestion);
                }
            }

            categoryQuestion.Category = category;

            if (categoryQuestion.Category != null)
            {
                if (!categoryQuestion.Category.CategoryQuestions.Contains(categoryQuestion))
                {
                    categoryQuestion.Category.CategoryQuestions.Add(categoryQuestion);
                }
            }
        }

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

        public static void LinkTo(this QuestionFlag questionFlag, Question question)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.Question != null)
            {
                if (questionFlag.Question.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.Question.QuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.Question = question;

            if (questionFlag.Question != null)
            {
                if (!questionFlag.Question.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.Question.QuestionFlags.Add(questionFlag);
                }
            }
        }

        public static void LinkTo(this QuestionFlag questionFlag, FlagStatus flagStatus)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.FlagStatus != null)
            {
                if (questionFlag.FlagStatus.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlagStatus.QuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.FlagStatus = flagStatus;

            if (questionFlag.FlagStatus != null)
            {
                if (!questionFlag.FlagStatus.QuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlagStatus.QuestionFlags.Add(questionFlag);
                }
            }
        }

        public static void LinkToFlaggedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.FlaggedByUser != null)
            {
                if (questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.FlaggedByUser = user;

            if (questionFlag.FlaggedByUser != null)
            {
                if (!questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.FlaggedByUser.AsFlaggedByInQuestionFlags.Add(questionFlag);
                }
            }
        }

        public static void LinkToLastModifiedByUser(this QuestionFlag questionFlag, User user)
        {
            if (questionFlag == null) throw new ArgumentNullException("questionFlag");

            if (questionFlag.LastModifiedByUser != null)
            {
                if (questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Remove(questionFlag);
                }
            }

            questionFlag.LastModifiedByUser = user;

            if (questionFlag.LastModifiedByUser != null)
            {
                if (!questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Contains(questionFlag))
                {
                    questionFlag.LastModifiedByUser.AsLastModifiedByInQuestionFlags.Add(questionFlag);
                }
            }
        }

        public static void LinkTo(this QuestionLink questionLink, Question question)
        {
            if (questionLink == null) throw new ArgumentNullException("questionLink");

            if (questionLink.Question != null)
            {
                if (!questionLink.Question.QuestionLinks.Contains(questionLink))
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

        public static void LinkTo(this QuestionType questionType, Question question)
        {
            if (questionType == null) { throw new ArgumentNullException("questionType"); }

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

        public static void LinkTo(this Source source, Question question)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

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
