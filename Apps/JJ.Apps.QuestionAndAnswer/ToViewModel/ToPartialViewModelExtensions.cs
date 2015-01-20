using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ToViewModel
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
