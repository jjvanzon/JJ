using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels.Entities
{
    public sealed class CurrentUserQuestionFlagPartialViewModel
    {
        public string Comment { get; set; }
        public bool CanFlag { get; set; }
        public bool IsFlagged { get; set; }
    }
}

