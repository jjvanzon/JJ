using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using System.Collections.Generic;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels.Partials
{
    public sealed class LanguageSelectorPartialViewModel
    {
        public string SelectedLanguageCultureName { get; set; }
        public IList<LanguageViewModel> Languages { get; set; }
    }
}
