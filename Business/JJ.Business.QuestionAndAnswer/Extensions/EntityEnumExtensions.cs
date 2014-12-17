using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Framework.Reflection;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class EntityEnumExtensions
    {
        public static QuestionTypeEnum GetQuestionTypeEnum(this Question entity)
        {
            return (QuestionTypeEnum)entity.QuestionType.ID;
        }

        public static void SetQuestionTypeEnum(this Question entity, QuestionTypeEnum value, IQuestionTypeRepository questionTypeRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (questionTypeRepository == null) throw new NullException(() => questionTypeRepository);

            QuestionType questionType = questionTypeRepository.Get((int)value);
            questionType.LinkTo(entity);
        }

        public static FlagStatusEnum GetFlagStatusEnum(this QuestionFlag entity)
        {
            return (FlagStatusEnum)entity.FlagStatus.ID;
        }

        public static void SetFlagStatusEnum(this QuestionFlag entity, FlagStatusEnum value, IFlagStatusRepository flagStatusRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (flagStatusRepository == null) throw new NullException(() => flagStatusRepository);

            FlagStatus flagStatus = flagStatusRepository.Get((int)value);
            entity.LinkTo(flagStatus);
        }
    }
}
