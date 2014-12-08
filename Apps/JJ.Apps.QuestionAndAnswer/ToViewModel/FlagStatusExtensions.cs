using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ToViewModel
{
    internal static class FlagStatusExtensions
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
