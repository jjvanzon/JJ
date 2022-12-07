namespace JJ.Data.QuestionAndAnswer
{
    public class RunCategory
    {
        public virtual int ID { get; set; }
        public virtual Run Run { get; set; }
        public virtual Category Category { get; set; }
    }
}
