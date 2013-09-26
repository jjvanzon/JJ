using System;
namespace JJ.Models.QuestionAndAnswer
{
    public class Answer
    {
        private int _iD;
        private bool _isCorrectAnswer;
        private Question _question;
        private string _text;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual bool IsCorrectAnswer
        {
            get { return _isCorrectAnswer; }
            set
            { _isCorrectAnswer = value; }
        }

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public virtual string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
