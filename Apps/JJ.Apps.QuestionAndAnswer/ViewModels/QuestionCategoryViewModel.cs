using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionCategoryViewModel
    {
        /// <summary> Available for both committed and newly added entities. </summary>
        public Guid TemporaryID { get; set; }

        /// <summary> 0 for newly added items. </summary>
        public int QuestionCategoryID { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}
