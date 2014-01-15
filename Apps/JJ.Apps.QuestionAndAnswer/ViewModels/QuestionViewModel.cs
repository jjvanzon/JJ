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
        
        public List<QuestionLinkViewModel> Links { get; set; }
        public List<QuestionCategoryViewModel> Categories { get; set; }

        /// <summary> Not available in RandomQuestionViewModel. </summary>
        public List<QuestionFlagViewModel> Flags { get; set; }

        public bool IsFlagged { get; set; }
    }
}
