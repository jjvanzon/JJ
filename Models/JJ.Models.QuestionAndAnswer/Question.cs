using System;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer
{
    public class Question
    {
        private int _iD;
        private IList<Answer> _answers = new List<Answer>();
        private IList<QuestionCategory> _questionCategories = new List<QuestionCategory>();
        private IList<QuestionLink> _questionLinks = new List<QuestionLink>();
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
