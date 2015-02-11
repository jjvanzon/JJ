using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class QuestionCategory
    {

        private System.Int32 _id;
        private Category _category;
        private Question _question;

        public virtual System.Int32 Id
        {
            get
            {
                return _id;
            }
        }

        public virtual Category Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
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







    }
}
