using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class Run
    {

        private System.Int32 _id;
        private System.String _description;
        private System.Boolean _isActive;
        private System.Collections.IList _runCategories;
        private User _user;
        private System.Collections.IList _userAnswers;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual System.String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public virtual System.Boolean IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }

        public virtual System.Collections.IList RunCategories
        {
            get
            {
                return _runCategories;
            }
            set
            {
                _runCategories = value;
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

        public virtual System.Collections.IList UserAnswers
        {
            get
            {
                return _userAnswers;
            }
            set
            {
                _userAnswers = value;
            }
        }







    }
}
