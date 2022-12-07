using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
    public class QuestionType
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Question> Questions { get; set; } = new List<Question>();
    }
}
