using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
	public class Source
	{
		public virtual int ID { get; set; }

		public virtual string Description { get; set; }
		public virtual string Identifier { get; set; }
		public virtual string Url { get; set; }
		public virtual bool IsActive { get; set; }

		public virtual IList<Question> Questions { get; set; } = new List<Question>();
	}
}