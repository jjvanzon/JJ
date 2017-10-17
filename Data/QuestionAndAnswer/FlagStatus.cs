using System.Collections.Generic;
using System.Diagnostics;

namespace JJ.Data.QuestionAndAnswer
{
    [DebuggerDisplay("{ID} - {Description}")]
    public class FlagStatus
    {
        public virtual int ID { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<QuestionFlag> QuestionFlags { get; set; } = new List<QuestionFlag>();
    }
}
