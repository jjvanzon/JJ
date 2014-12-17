using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ToViewModel
{
    internal static class QuestionTypeExtensions
    {
        public static QuestionTypeViewModel ToViewModel(this QuestionType entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return new QuestionTypeViewModel 
            {
                ID = entity.ID,
                Name = entity.Name
            };
        }
    }
}
