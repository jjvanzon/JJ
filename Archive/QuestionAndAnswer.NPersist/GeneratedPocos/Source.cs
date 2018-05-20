using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class Source
    {

        private System.Int32 _id;
        private System.String _description;
        private System.String _identifier;
        private System.Collections.IList _questions;
        private System.Boolean _isActive;
        private System.String _url;

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

        public virtual System.String Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                _identifier = value;
            }
        }

        public virtual System.Collections.IList Questions
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
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

        public virtual System.String Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }







    }
}
