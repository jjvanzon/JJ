using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Models.QuestionAndAnswer
{
    public class Source
    {
        private int _iD;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        private string _identifier;

        public virtual string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        private string _description;

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _link;

        public virtual string Link
        {
            get { return _link; }
            set { _link = value; }
        }
    }
}
