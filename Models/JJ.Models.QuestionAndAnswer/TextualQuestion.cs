using System;

namespace JJ.Models.QuestionAndAnswer
{
    public class TextualQuestion
    {
        private int _iD;

        public virtual int ID 
        {
            get { return _iD; }
            set { _iD = value; }
        }

        private string _text;

        public virtual string Text 
        {
            get { return _text; }
            set { _text = value; }
        }

        private Category _category;

        public virtual Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private Source _source;

        public virtual Source Source
        {
            get { return _source; }
            set { _source = value; }
        }
        /// <summary>
        /// not nullable
        /// </summary>
        public virtual TextualAnswer TextualAnswer { get; set; }
    }
}