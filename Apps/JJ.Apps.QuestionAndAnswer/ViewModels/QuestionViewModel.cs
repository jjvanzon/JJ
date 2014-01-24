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
        
        public IList<QuestionLinkViewModel> Links { get; set; }
        public IList<QuestionCategoryViewModel> Categories { get; set; }

        /// <summary> Not available in RandomQuestionViewModel. </summary>
        public IList<QuestionFlagViewModel> Flags { get; set; }

        public bool IsFlagged { get; set; }

        public SourceViewModel Source { get; set; }
        public QuestionTypeViewModel Type { get; set; }

        internal bool IsDirty { get; set; }
    }
}
