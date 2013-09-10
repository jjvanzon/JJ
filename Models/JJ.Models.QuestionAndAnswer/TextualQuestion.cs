using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer
{
    public class TextualQuestion// : IQuestion
    {
        public virtual int ID { get; set; }

        public virtual string Text { get; set; }
        /*public virtual TextualAnswer TextualAnswer { get; set; }

        string IQuestion.Text
        {
            get { return Text; }
            set { Text = value; }
        }

        IAnswer IQuestion.Answer
        {
            get { return TextualAnswer; }
            set { TextualAnswer = (TextualAnswer)value; }
        }*/
    }
}
