using System;
namespace JJ.Models.QuestionAndAnswer
{

    public class RunCategory
    {

        private System.Int32 _id;
        private Category _category;
        private Run _run;

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

        public virtual Run Run
        {
            get
            {
                return _run;
            }
            set
            {
                _run = value;
            }
        }







    }
}
