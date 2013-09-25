using System;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer
{
    public class TextualAnswer
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

        private TextualQuestion _textualQuestion;

        public virtual TextualQuestion TextualQuestion
        {
            get { return _textualQuestion; }
            set { _textualQuestion = value; }
        }

        /*public virtual TextualQuestion TextualQuestion 
        {
            get
            {
                return _textualQuestion;
            }
            set
            {
                if (_textualQuestion == value)
                {
                    return;
                }

                if (_textualQuestion != null)
                {
                    if (_textualQuestion.TextualAnswers.Contains(this))
                    {
                        _textualQuestion.TextualAnswers.Remove(this);
                    }
                }

                _textualQuestion = value;

                if (_textualQuestion != null)
                {
                    if (!_textualQuestion.TextualAnswers.Contains(this))
                    {
                        _textualQuestion.TextualAnswers.Add(this);
                    }
                }
            }
        }*/
    }
}