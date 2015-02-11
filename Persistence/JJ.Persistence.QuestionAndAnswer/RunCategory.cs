using System;

namespace JJ.Persistence.QuestionAndAnswer
{
    public class RunCategory
    {
        private int _iD;
        private Run _run;
        private Category _category;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual Run Run
        {
            get { return _run; }
            set { _run = value; }
        }

        public virtual Category Category
        {
            get { return _category; }
            set { _category = value; }
        }
    }
}
