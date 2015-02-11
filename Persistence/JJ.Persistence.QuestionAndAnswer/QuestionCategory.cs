using System;

namespace JJ.Persistence.QuestionAndAnswer
{
    public class QuestionCategory
    {
        private int _iD;
        private Category _category;
        private Question _question;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }
    }
}
