using System;
using System.Collections.Generic;

namespace JJ.Persistence.QuestionAndAnswer
{
    public class User
    {
        private int _iD;
        private string _displayName;
        private string _userName;
        private string _password;
        private string _securitySalt;

        private IList<Run> _runs = new List<Run>();
        private IList<UserAnswer> _userAnswers = new List<UserAnswer>();
        private IList<Question> _asLastModifiedByInQuestions = new List<Question>();
        private IList<QuestionFlag> _asFlaggedByInQuestionFlags = new List<QuestionFlag>();
        private IList<QuestionFlag> _asLastModifiedByInQuestionFlags = new List<QuestionFlag>();

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public virtual string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public virtual string SecuritySalt
        {
            get { return _securitySalt; }
            set { _securitySalt = value; }
        }

        public virtual IList<Run> Runs
        {
            get { return _runs; }
            set { _runs = value; }
        }

        public virtual IList<UserAnswer> UserAnswers
        {
            get { return _userAnswers; }
            set { _userAnswers = value; }
        }

        public virtual IList<QuestionFlag> AsLastModifiedByInQuestionFlags
        {
            get { return _asLastModifiedByInQuestionFlags; }
            set { _asLastModifiedByInQuestionFlags = value; }
        }

        public virtual IList<QuestionFlag> AsFlaggedByInQuestionFlags
        {
            get { return _asFlaggedByInQuestionFlags; }
            set { _asFlaggedByInQuestionFlags = value; }
        }

        public virtual IList<Question> AsLastModifiedByInQuestions
        {
            get { return _asLastModifiedByInQuestions; }
            set { _asLastModifiedByInQuestions = value; }
        }
    }
}