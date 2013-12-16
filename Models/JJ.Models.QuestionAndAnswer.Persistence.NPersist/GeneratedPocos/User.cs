using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class User
    {

        private System.Int32 _id;
        private System.String _name;
        private System.String _password;
        private System.Collections.IList _runs;
        private System.String _userName;
        private System.Collections.IList _userAnswers;
        private System.Collections.IList _asLastModifiedByInQuestionFlags;
        private System.Collections.IList _asFlaggedByInQuestionFlags;
        private System.Collections.IList _asLastModifiedByInQuestions;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual System.String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public virtual System.String Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public virtual System.Collections.IList Runs
        {
            get
            {
                return _runs;
            }
            set
            {
                _runs = value;
            }
        }

        public virtual System.String UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public virtual System.Collections.IList UserAnswers
        {
            get
            {
                return _userAnswers;
            }
            set
            {
                _userAnswers = value;
            }
        }

        public virtual System.Collections.IList AsLastModifiedByInQuestionFlags
        {
            get
            {
                return _asLastModifiedByInQuestionFlags;
            }
            set
            {
                _asLastModifiedByInQuestionFlags = value;
            }
        }

        public virtual System.Collections.IList AsFlaggedByInQuestionFlags
        {
            get
            {
                return _asFlaggedByInQuestionFlags;
            }
            set
            {
                _asFlaggedByInQuestionFlags = value;
            }
        }

        public virtual System.Collections.IList AsLastModifiedByInQuestions
        {
            get
            {
                return _asLastModifiedByInQuestions;
            }
            set
            {
                _asLastModifiedByInQuestions = value;
            }
        }







    }
}
