using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Models.QuestionAndAnswer
{
    public class Category
    {
        private int _iD;

        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        private string _name;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
