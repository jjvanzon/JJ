using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
	public class Run
	{
		public virtual int ID { get; set; }
		public virtual string Description { get; set; }
		public virtual bool IsActive { get; set; }

		public virtual User User { get; set; }
		public virtual IList<RunCategory> RunCategories { get; set; } = new List<RunCategory>();
		public virtual IList<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
	}
}
