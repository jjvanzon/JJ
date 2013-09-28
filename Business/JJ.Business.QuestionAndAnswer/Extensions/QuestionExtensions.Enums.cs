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

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static partial class QuestionExtensions
    {
        public static void SetSourceEnum(this Question entity, ISourceRepository sourceRepository, SourceEnum value)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (sourceRepository == null) throw new ArgumentNullException("sourceRepository");

            Source source = sourceRepository.Get((int)value);
            source.LinkTo(entity);
        }

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
