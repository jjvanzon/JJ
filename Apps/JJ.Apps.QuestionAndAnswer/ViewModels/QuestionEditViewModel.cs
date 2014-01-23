using JJ.Models.Canonical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionEditViewModel
    {
        public QuestionViewModel Question { get; set; }

        public IList<FlagStatusViewModel> FlagStatuses { get; set; }

        public IList<CategoryViewModel> Categories { get; set; }

        public IList<ValidationMessage> ValidationMessages { get; set; }
    }
}
