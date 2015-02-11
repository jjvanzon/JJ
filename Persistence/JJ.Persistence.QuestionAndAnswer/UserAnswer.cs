using System;

namespace JJ.Persistence.QuestionAndAnswer
{
    public class UserAnswer
    {
        private int _iD;
        private DateTime _dateTime;

        private Question _question;
        private AnswerStatus _answerStatus;
        private User _user;
        private Run _run;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public virtual AnswerStatus AnswerStatus
        {
            get { return _answerStatus; }
            set { _answerStatus = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public virtual Run Run
        {
            get { return _run; }
            set { _run = value; }
        }
    }
}
