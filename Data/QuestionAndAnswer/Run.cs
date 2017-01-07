using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
    public class Run
    {
        private int _iD;
        private string _description;
        private bool _isActive;

        private User _user;

        private IList<UserAnswer> _userAnswers = new List<UserAnswer>();
        private IList<RunCategory> _runCategories = new List<RunCategory>();

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

        public virtual bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public virtual IList<RunCategory> RunCategories
        {
            get { return _runCategories; }
            set { _runCategories = value; }
        }

        public virtual IList<UserAnswer> UserAnswers
        {
            get { return _userAnswers; }
            set { _userAnswers = value; }
        }
    }
}
