using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using JJ.Framework.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class LoginViewModel
    {
        public LanguageSelectorPartialViewModel LanguageSelector { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecurityToken { get; set; }
        public ActionDescriptor ReturnAction { get; set; }
    }
}
