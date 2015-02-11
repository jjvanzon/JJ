using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JJ.Persistence.QuestionAndAnswer
{
    [DebuggerDisplay("{ID} - {Description}")]
    public class FlagStatus
    {
        private int _iD;
        private string _description;
        private IList<QuestionFlag> _questionFlags = new List<QuestionFlag>();

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

        public virtual IList<QuestionFlag> QuestionFlags
        {
            get { return _questionFlags; }
            set { _questionFlags = value; }
        }
    }
}
