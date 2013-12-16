using System;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer
{
    public class QuestionFlag
    {
        private int _iD;
        private string _comment;
        private DateTime _dateTime;

        private Question _question;
        private User _flaggedByUser;
        private User _lastModifiedByUser;
        private FlagStatus _flagStatus;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public virtual User FlaggedByUser
        {
            get { return _flaggedByUser; }
            set { _flaggedByUser = value; }
        }

        public virtual User LastModifiedByUser
        {
            get { return _lastModifiedByUser; }
            set { _lastModifiedByUser = value; }
        }

        public virtual FlagStatus FlagStatus
        {
            get { return _flagStatus; }
            set { _flagStatus = value; }
        }

        public virtual string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public virtual DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }
    }
}