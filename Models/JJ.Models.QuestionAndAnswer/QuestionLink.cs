using System;

namespace JJ.Models.QuestionAndAnswer
{
    public class QuestionLink
    {
        private int _iD;
        private string _link;
        private Question _question;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Link
        {
            get { return _link; }
            set { _link = value; }
        }

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }
    }
}
