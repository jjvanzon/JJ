using System;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer
{
    public class TextualQuestion
    {
        private int _iD;

        public virtual int ID 
        {
            get { return _iD; }
            set { _iD = value; }
        }

        private string _text;

        public virtual string Text 
        {
            get { return _text; }
            set { _text = value; }
        }

        private Category _category;

        public virtual Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private Source _source;

        public virtual Source Source
        {
            get { return _source; }
            set { _source = value; }
        }

        private IList<TextualAnswer> _textualAnswers = new List<TextualAnswer>();

        public virtual IList<TextualAnswer> TextualAnswers
        {
            get { return _textualAnswers; }
            set { _textualAnswers = value; }
        }
        
        /*public virtual TextualAnswer TextualAnswer
        {
            get
            {
                return _textualAnswer;
            }
            set
            {
                if (_textualAnswer == value)
                {
                    return;
                }

                if (_textualAnswer != null)
                {
                    _textualAnswer.TextualQuestion = null;
                }

                _textualAnswer = value;

                if (_textualAnswer != null)
                {
                    _textualAnswer.TextualQuestion = this;
                }
            }
        }*/
    }
}