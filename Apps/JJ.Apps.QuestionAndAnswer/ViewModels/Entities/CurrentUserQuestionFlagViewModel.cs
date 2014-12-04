using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Entities
{
    public class CurrentUserQuestionFlagViewModel
    {
        public string Comment { get; set; }
        public bool CanFlag { get; set; }
        public bool IsFlagged { get; set; }
    }
}

