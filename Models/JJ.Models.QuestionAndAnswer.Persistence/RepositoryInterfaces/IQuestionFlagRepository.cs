using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface IQuestionFlagRepository : IRepository<QuestionFlag, int>
    {
        QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID);
    }
}
