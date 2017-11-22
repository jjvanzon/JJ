using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
	public class AnswerStatus
	{
		public virtual int ID { get; set; }
		public virtual string Description { get; set; }
		public virtual IList<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
	}
}