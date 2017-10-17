using System;

namespace JJ.Data.QuestionAndAnswer
{
    public class UserAnswer
    {
        public virtual int ID { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual Question Question { get; set; }
        public virtual AnswerStatus AnswerStatus { get; set; }
        public virtual User User { get; set; }
        public virtual Run Run { get; set; }
    }
}
