using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    public static class QuestionLinkExtensions
    {
        public static QuestionLinkViewModel ToViewModel(this QuestionLink entity)
        {
            if (entity == null) { throw new ArgumentNullException("entity"); }

            return new QuestionLinkViewModel
            {
                ID = entity.ID,
                TemporaryID = Guid.NewGuid(),
                Description = entity.Description,
                Url = entity.Url
            };
        }
    }
}
