using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
    public class QuestionType
    {
        private int _iD;
        private string _name;
        private IList<Question> _questions = new List<Question>();

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual IList<Question> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }
    }
}
