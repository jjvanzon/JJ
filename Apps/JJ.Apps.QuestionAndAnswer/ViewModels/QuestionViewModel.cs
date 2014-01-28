using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionViewModel
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }

        public string LastModifiedBy { get; set; }
        public bool IsManual { get; set; }

        public SourceViewModel Source { get; set; }
        public QuestionTypeViewModel Type { get; set; }

        public ListViewModel<QuestionLinkViewModel> Links { get; set; }
        public ListViewModel<QuestionCategoryViewModel> Categories { get; set; }

        public bool IsFlagged { get; set; }

        /// <summary> Not available in RandomQuestionViewModel. </summary>
        public ListViewModel<QuestionFlagViewModel> Flags { get; set; }

        internal bool IsDirty { get; set; }
        internal bool IsNew { get; set; }
    }
}
