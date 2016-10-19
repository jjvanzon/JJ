using JJ.Framework.Presentation;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class QuestionListViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public IList<QuestionViewModel> List { get; set; }
        public PagerViewModel Pager { get; set; }
    }
}
