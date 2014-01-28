using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionTypeViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        internal bool IsDirty { get; set; }
        internal bool IsNew { get; set; }
    }
}
