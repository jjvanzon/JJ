using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class CategoryExtensions_LinkTo
    {
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
    }
}
