using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class CategoryExtensions
    {
        public static bool IsLeaf(this Category category)
        {
            if (category == null) throw new NullException(() => category);
            return category.SubCategories.Count == 0;
        }

        public static bool IsRoot(this Category category)
        {
            if (category == null) throw new NullException(() => category);
            return category.ParentCategory == null;
        }
    }
}
