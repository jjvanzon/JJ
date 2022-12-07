using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class QuestionType
    {

        private System.Int32 _id;
        private System.String _name;
        private System.Collections.IList _questions;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public virtual System.String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
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







    }
}
