using System.Collections.Generic;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class QuestionEditViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public QuestionViewModel Question { get; set; }
        public IList<string> ValidationMessages { get; set; }
        public string Title { get; set; }
        public bool IsNew { get; set; }
        public bool CanDelete { get; set; }
        public bool Successful { get; set; }
        public string ReturnAction { get; set; }
    }
}