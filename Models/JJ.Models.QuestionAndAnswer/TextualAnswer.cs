using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer
{
    public class TextualAnswer : IAnswer
    {
        public virtual int ID { get; set; }
        public virtual string Text { get; set; }
    }
}
