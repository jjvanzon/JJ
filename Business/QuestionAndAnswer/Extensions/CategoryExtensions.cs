using JJ.Framework.Exceptions;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;

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
