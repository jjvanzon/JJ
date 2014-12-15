using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.QuestionAndAnswer.LinkTo;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionFlagExtensions_Enums
    {
        public static void SetFlagStatusEnum(this QuestionFlag entity, IFlagStatusRepository flagStatusRepository, FlagStatusEnum value)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");

            FlagStatus flagStatus = flagStatusRepository.Get((int)value);
            entity.LinkTo(flagStatus);
        }
    }
}
