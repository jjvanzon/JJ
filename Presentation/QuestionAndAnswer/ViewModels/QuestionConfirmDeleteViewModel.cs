using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class QuestionConfirmDeleteViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public int ID { get; set; }
        public string Question { get; set; }
    }
}
