using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Extensions
{
    internal static class QuestionTypeExtensions_ToViewModel
    {
        public static QuestionTypeViewModel ToViewModel(this QuestionType entity)
        {
            if (entity == null) { throw new ArgumentNullException("entity"); }

            return new QuestionTypeViewModel 
            {
                ID = entity.ID,
                Name = entity.Name
            };
        }
    }
}
