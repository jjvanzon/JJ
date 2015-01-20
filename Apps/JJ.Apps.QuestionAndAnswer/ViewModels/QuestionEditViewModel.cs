using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Models.Canonical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public sealed class QuestionEditViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public LanguageSelectorPartialViewModel LanguageSelector { get; set; }

        public QuestionViewModel Question { get; set; }

        public IList<FlagStatusViewModel> FlagStatuses { get; set; }
        public IList<CategoryViewModel> Categories { get; set; }
        public IList<ValidationMessage> ValidationMessages { get; set; }

        public string Title { get; set; }
        public bool IsNew { get; set; }
        public bool CanDelete { get; set; }
    }
}
