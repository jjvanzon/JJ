using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
    public class AnswerStatus
    {
        private int _iD;
        private string _description;
        private IList<UserAnswer> _userAnswers = new List<UserAnswer>();

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual IList<UserAnswer> UserAnswers
        {
            get { return _userAnswers; }
            set { _userAnswers = value; }
        }
    }
}