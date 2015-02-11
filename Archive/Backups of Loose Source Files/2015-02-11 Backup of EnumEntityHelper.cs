using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Helpers
{
    public static class EnumEntityHelper
    {
        public static QuestionType GetQuestionType(QuestionTypeEnum questionTypeEnum, IQuestionTypeRepository questionTypeRepository)
        {
            if (questionTypeRepository == null) throw new NullException(() => questionTypeRepository);
            return questionTypeRepository.Get((int)questionTypeEnum);
        }
    }
}
