namespace JJ.Data.QuestionAndAnswer
{
	public class Answer
	{
		public virtual int ID { get; set; }
		public virtual bool IsCorrectAnswer { get; set; }
		public virtual Question Question { get; set; }
		public virtual string Text { get; set; }
	}
}
