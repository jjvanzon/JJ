using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Partials
{
    public sealed class LoginPartialViewModel
    {
        public string UserDisplayName { get; set; }
        public bool CanLogIn { get; set; }
        public bool CanLogOut { get; set; }
    }
}
