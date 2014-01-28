using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class SourceViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        internal bool IsDirty { get; set; }
        internal bool IsNew { get; set; }
    }
}
