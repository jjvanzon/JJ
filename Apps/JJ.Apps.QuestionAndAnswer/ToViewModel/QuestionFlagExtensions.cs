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
    internal static class QuestionFlagExtensions
    {
        public static CurrentUserQuestionFlagViewModel ToCurrentUserQuestionFlagViewModel(this QuestionFlag entity)
        {
            if (entity == null) throw new Exception("entity");

            return new CurrentUserQuestionFlagViewModel
            {
                CanFlag = true,
                IsFlagged = true,
                Comment = entity.Comment
            };
        }

        public static QuestionFlagViewModel ToViewModel(this QuestionFlag entity)
        {
            if (entity == null) throw new Exception("entity");

            return new QuestionFlagViewModel
            {
                ID = entity.ID,
                Comment = entity.Comment,
                DateAndTime = entity.DateTime,
                FlaggedBy = entity.FlaggedByUser.DisplayName,
                LastModifiedBy = entity.LastModifiedByUser.DisplayName,
                Status = entity.FlagStatus.ToViewModel()
            };
        }
    }
}
