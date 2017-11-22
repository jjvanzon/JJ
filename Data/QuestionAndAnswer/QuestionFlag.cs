using System;

namespace JJ.Data.QuestionAndAnswer
{
	public class QuestionFlag
	{
		public virtual int ID { get; set; }
		public virtual Question Question { get; set; }
		public virtual User FlaggedByUser { get; set; }
		public virtual User LastModifiedByUser { get; set; }
		public virtual FlagStatus FlagStatus { get; set; }
		public virtual string Comment { get; set; }
		public virtual DateTime DateTime { get; set; }
	}
}