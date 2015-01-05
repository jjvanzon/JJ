using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public sealed class LanguageSelectorViewModel
    {
        public string SelectedLanguageCultureName { get; set; }
        public IList<LanguageViewModel> Languages { get; set; }
    }
}
