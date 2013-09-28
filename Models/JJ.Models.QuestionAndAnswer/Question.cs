using System;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer
{
    public class Question
    {
        private int _iD;
        private IList<Answer> _answers;
        private IList<QuestionCategory> _questionCategories;
        private IList<QuestionLink> _questionLinks;
        private QuestionType _questionType;
        private Source _source;
        private string _text;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual IList<Answer> Answers
        {
            get
            {
                if (_answers == null)
                {
                    _answers = new List<Answer>();
                }

                return _answers;
            }
            set
            {
                _answers = value;
            }
        }

        public virtual IList<QuestionCategory> QuestionCategories
        {
            get
            {
                if (_questionCategories == null)
                {
                    _questionCategories = new List<QuestionCategory>();
                }

                return _questionCategories;
            }
            set
            {
                _questionCategories = value;
            }
        }

        public virtual IList<QuestionLink> QuestionLinks
        {
            get
            {
                if (_questionLinks == null)
                {
                    _questionLinks = new List<QuestionLink>();
                }

                return _questionLinks;
            }
            set
            {
                _questionLinks = value;
            }
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

        public virtual string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
