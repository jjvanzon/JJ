using System;
using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer
{
    public class Question
    {
        private int _iD;

        private string _text;
        private bool _isManual;
        private bool _isActive;

        private QuestionType _questionType;
        private Source _source;
        private User _lastModifiedByUser;

        private IList<Answer> _answers = new List<Answer>();
        private IList<QuestionCategory> _questionCategories = new List<QuestionCategory>();
        private IList<QuestionLink> _questionLinks = new List<QuestionLink>();
        private IList<UserAnswer> _userAnswers = new List<UserAnswer>();
        private IList<QuestionFlag> _questionFlags = new List<QuestionFlag>();

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public virtual bool IsManual
        {
            get { return _isManual; }
            set { _isManual = value; }
        }
        
        public virtual bool IsActive 
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public virtual QuestionType QuestionType
        {
            get { return _questionType; }
            set { _questionType = value; }
        }

        public virtual Source Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        /// nullable
        /// </summary>
        public virtual User LastModifiedByUser
        {
            get { return _lastModifiedByUser; }
            set { _lastModifiedByUser = value; }
        }

        public virtual IList<Answer> Answers
        {
            get { return _answers; }
            set { _answers = value; }
        }

        public virtual IList<QuestionCategory> QuestionCategories
        {
            get { return _questionCategories; }
            set { _questionCategories = value; }
        }

        public virtual IList<QuestionLink> QuestionLinks
        {
            get { return _questionLinks; }
            set { _questionLinks = value; }
        }

        public virtual IList<UserAnswer> UserAnswers
        {
            get { return _userAnswers; }
            set { _userAnswers = value; }
        }

        public virtual IList<QuestionFlag> QuestionFlags
        {
            get { return _questionFlags; }
            set { _questionFlags = value; }
        }
    }
}
