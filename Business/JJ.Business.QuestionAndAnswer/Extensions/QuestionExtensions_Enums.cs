using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using System.Collections;
using JJ.Business.QuestionAndAnswer.LinkTo;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionExtensions_Enums
    {
        public static QuestionTypeEnum GetQuestionTypeEnum(this Question entity)
        {
            return (QuestionTypeEnum)entity.QuestionType.ID;
        }

        public static void SetQuestionTypeEnum(this Question entity, IQuestionTypeRepository questionTypeRepository, QuestionTypeEnum value)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (questionTypeRepository == null) throw new ArgumentNullException("questionTypeRepository");

            QuestionType questionType = questionTypeRepository.Get((int)value);
            questionType.LinkTo(entity);
        }
    }
}
