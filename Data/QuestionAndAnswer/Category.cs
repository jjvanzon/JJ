using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
	public class Category
	{
		public virtual int ID { get; set; }

		public virtual string Description { get; set; }
		public virtual string Identifier { get; set; }
		public virtual bool IsActive { get; set; }

		public virtual Category ParentCategory { get; set; }
		public virtual IList<Category> SubCategories { get; set; } = new List<Category>();
		public virtual IList<QuestionCategory> CategoryQuestions { get; set; } = new List<QuestionCategory>();
		public virtual IList<RunCategory> CategoryRuns { get; set; } = new List<RunCategory>();
	}
}
