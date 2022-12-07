using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class AnswerHistory
    {

        private System.Int32 _id;
        private AnswerStatus _answerStatus;
        private System.DateTime _dateTime;
        private Question _question;
        private Run _run;
        private User _user;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual AnswerStatus AnswerStatus
        {
            get
            {
                return _answerStatus;
            }
            set
            {
                _answerStatus = value;
            }
        }

        public virtual System.DateTime DateTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value;
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

        public virtual Run Run
        {
            get
            {
                return _run;
            }
            set
            {
                _run = value;
            }
        }

        public virtual User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }







    }
}
