using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class FlagStatus
    {

        private System.Int32 _id;
        private System.String _description;
        private System.Collections.IList _questionFlags;

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

        public virtual System.Collections.IList QuestionFlags
        {
            get
            {
                return _questionFlags;
            }
            set
            {
                _questionFlags = value;
            }
        }







    }
}
