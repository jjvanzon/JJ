using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class QuestionDetailsViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public QuestionViewModel Question { get; set; }
    }
}
