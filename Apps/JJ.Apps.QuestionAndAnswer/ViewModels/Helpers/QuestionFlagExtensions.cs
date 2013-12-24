using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class QuestionFlagExtensions
    {
        public static QuestionFlagViewModel ToViewModel(this QuestionFlag entity)
        {
            if (entity == null) throw new Exception("entity");

            return new QuestionFlagViewModel
            {
                CanFlag = true,
                IsFlagged = true,
                Comment = entity.Comment
            };
        }
    }
}
