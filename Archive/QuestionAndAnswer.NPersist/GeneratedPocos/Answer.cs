using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class Answer
    {

        private System.Int32 _id;
        private System.Boolean _isCorrectAnswer;
        private Question _question;
        private System.String _text;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual System.Boolean IsCorrectAnswer
        {
            get
            {
                return _isCorrectAnswer;
            }
            set
            {
                _isCorrectAnswer = value;
            }
        }

        public virtual Question Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
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







    }
}
