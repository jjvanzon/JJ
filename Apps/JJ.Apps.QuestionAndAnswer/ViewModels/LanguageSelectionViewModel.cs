using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class LanguageSelectionViewModel
    {
        public string SelectedLanguageCultureName { get; set; }
        public IList<LanguageViewModel> Languages { get; set; }
    }
}
