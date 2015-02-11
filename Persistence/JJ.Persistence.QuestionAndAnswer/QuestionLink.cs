using System;

namespace JJ.Persistence.QuestionAndAnswer
{
    public class QuestionLink
    {
        private int _iD;
        private string _url;
        private string _description;
        private Question _question;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }
    }
}
