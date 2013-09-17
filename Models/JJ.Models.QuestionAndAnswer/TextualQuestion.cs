using System;

namespace JJ.Models.QuestionAndAnswer
{
    public class TextualQuestion
    {
        private int _iD;

        public virtual int ID 
        {
            get { return _iD; }
            set { _iD = value; }
        }

        private string _text;

        public virtual string Text 
        {
            get { return _text; }
            set { _text = value; }
        }

        public virtual TextualAnswer TextualAnswer { get; set; }
    }
}