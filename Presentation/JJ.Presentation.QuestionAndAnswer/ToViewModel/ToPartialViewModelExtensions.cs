using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using JJ.Framework.Reflection;
using JJ.Persistence.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.QuestionAndAnswer.ToViewModel
{
    internal static class ToPartialViewModelExtensions
    {
        public static CurrentUserQuestionFlagPartialViewModel ToCurrentUserQuestionFlagViewModel(this QuestionFlag entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return new CurrentUserQuestionFlagPartialViewModel
            {
                CanFlag = true,
                IsFlagged = true,
                Comment = entity.Comment
            };
        }

        public static LoginPartialViewModel ToLoginPartialViewModel(this User user)
        {
            if (user == null) throw new NullException(() => user);

            return new LoginPartialViewModel
            {
                UserDisplayName = user.DisplayName,
                CanLogOut = true
            };
        }
    }
}
