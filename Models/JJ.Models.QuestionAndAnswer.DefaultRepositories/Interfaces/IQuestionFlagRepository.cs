using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces
{
    public interface IQuestionFlagRepository : IRepository<QuestionFlag, int>
    {
        QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID);
    }
}
