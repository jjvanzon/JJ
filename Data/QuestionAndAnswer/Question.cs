using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
	public class Question
	{
		public virtual int ID { get; set; }

		public virtual string Text { get; set; }
		public virtual bool IsManual { get; set; }
		public virtual bool IsActive { get; set; }

		public virtual QuestionType QuestionType { get; set; }
		public virtual Source Source { get; set; }

		/// <summary>
		/// nullable
		/// </summary>
		public virtual User LastModifiedByUser { get; set; }

		public virtual IList<Answer> Answers { get; set; } = new List<Answer>();
		public virtual IList<QuestionCategory> QuestionCategories { get; set; } = new List<QuestionCategory>();
		public virtual IList<QuestionLink> QuestionLinks { get; set; } = new List<QuestionLink>();
		public virtual IList<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
		public virtual IList<QuestionFlag> QuestionFlags { get; set; } = new List<QuestionFlag>();
	}
}
