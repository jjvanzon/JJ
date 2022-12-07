namespace JJ.Data.QuestionAndAnswer
{
    public class QuestionCategory
    {
        public virtual int ID { get; set; }
        public virtual Category Category { get; set; }
        public virtual Question Question { get; set; }
    }
}
