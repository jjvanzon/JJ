using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class AnswerStatus
    {

        private System.Int32 _id;
        private System.String _description;
        private System.Collections.IList _userAnswers;

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
