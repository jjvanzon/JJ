using System;
using System.Collections.Generic;

namespace JJ.Models.QuestionAndAnswer
{
    public class Source
    {
        private int _iD;
        private string _description;
        private string _identifier;
        private string _link;
        private IList<Question> _questions;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public virtual string Link
        {
            get { return _link; }
            set { _link = value; }
        }

        public virtual IList<Question> Questions
        {
            get
            {
                if (_questions == null)
                {
                    _questions = new List<Question>();
                }

                return _questions;
            }
            set
            {
                _questions = value;
            }
        }
    }
}