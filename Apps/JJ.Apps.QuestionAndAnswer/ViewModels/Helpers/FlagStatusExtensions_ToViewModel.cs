using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class FlagStatusExtensions_ToViewModel
    {
        public static FlagStatusViewModel ToViewModel(this FlagStatus entity)
        {
            if (entity == null) { throw new ArgumentNullException("entity"); }

            return new FlagStatusViewModel
            {
                ID = entity.ID,
                Description = entity.Description
            };
        }
    }
}
