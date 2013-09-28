using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static partial class CategoryExtensions
    {
        public static void LinkToParentCategory(this Category category, Category parentCategory)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }
            if (parentCategory == null)
            {
                throw new ArgumentNullException("parentCategory");
            }

            category.ParentCategory = parentCategory;
            parentCategory.SubCategories.Add(category);
        }

        public static void LinkToSubCategory(this Category category, Category subCategory)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }
            if (subCategory == null)
            {
                throw new ArgumentNullException("subCategory");
            }

            category.SubCategories.Add(subCategory);
            subCategory.ParentCategory = category;
        }

        public static void LinkTo(this Category category, QuestionCategory categoryQuestion)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }
            if (categoryQuestion == null)
            {
                throw new ArgumentNullException("categoryQuestion");
            }

            category.CategoryQuestions.Add(categoryQuestion);
            categoryQuestion.Category = category;
        }
    }
}
