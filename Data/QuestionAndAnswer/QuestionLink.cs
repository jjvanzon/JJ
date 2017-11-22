namespace JJ.Data.QuestionAndAnswer
{
	public class QuestionLink
	{
		public virtual int ID { get; set; }
		public virtual string Url { get; set; }
		public virtual string Description { get; set; }
		public virtual Question Question { get; set; }
	}
}
