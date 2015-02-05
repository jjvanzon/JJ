using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public sealed class QuestionListViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public IList<QuestionViewModel> List { get; set; }
        public PagingViewModel Paging { get; set; }
    }
}
