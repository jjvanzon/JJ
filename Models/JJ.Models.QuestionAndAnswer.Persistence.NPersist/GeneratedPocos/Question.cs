using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class Question
    {

        private System.Int32 _id;
        private System.Collections.IList _answers;
        private System.Collections.IList _questionCategories;
        private System.Collections.IList _questionLinks;
        private QuestionType _questionType;
        private Source _source;
        private System.String _text;
        private System.Boolean _isManual;
        private System.Collections.IList _questionFlags;
        private System.Collections.IList _userAnswers;
        private User _lastModifiedByUser;
        private System.Boolean _isActive;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual System.Collections.IList Answers
        {
            get
            {
                return _answers;
            }
            set
            {
                _answers = value;
            }
        }

        public virtual System.Collections.IList QuestionCategories
        {
            get
            {
                return _questionCategories;
            }
            set
            {
                _questionCategories = value;
            }
        }

        public virtual System.Collections.IList QuestionLinks
        {
            get
            {
                return _questionLinks;
            }
            set
            {
                _questionLinks = value;
            }
        }

        public virtual QuestionType QuestionType
        {
            get
            {
                return _questionType;
            }
            set
            {
                _questionType = value;
            }
        }

        public virtual Source Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        public virtual System.String Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public virtual System.Boolean IsManual
        {
            get
            {
                return _isManual;
            }
            set
            {
                _isManual = value;
            }
        }

        public virtual System.Collections.IList QuestionFlags
        {
            get
            {
                return _questionFlags;
            }
            set
            {
                _questionFlags = value;
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

        public virtual User LastModifiedByUser
        {
            get
            {
                return _lastModifiedByUser;
            }
            set
            {
                _lastModifiedByUser = value;
            }
        }

        public virtual System.Boolean IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }







    }
}
