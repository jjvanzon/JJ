using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class QuestionFlag
    {

        private System.Int32 _id;
        private System.String _comment;
        private System.DateTime _dateTime;
        private FlagStatus _flagStatus;
        private Question _question;
        private User _flaggedByUser;
        private User _lastModifiedByUser;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual System.String Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
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

        public virtual FlagStatus FlagStatus
        {
            get
            {
                return _flagStatus;
            }
            set
            {
                _flagStatus = value;
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

        public virtual User FlaggedByUser
        {
            get
            {
                return _flaggedByUser;
            }
            set
            {
                _flaggedByUser = value;
            }
        }

        public virtual User LastModifiedByUser
        {
            get
            {
                return _lastModifiedByUser;
            }
            set
            {
                _lastModifiedByUser = value;
            }
        }







    }
}
