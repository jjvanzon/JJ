using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class QuestionLink
    {

        private System.Int32 _id;
        private Question _question;
        private System.String _description;
        private System.String _url;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
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
