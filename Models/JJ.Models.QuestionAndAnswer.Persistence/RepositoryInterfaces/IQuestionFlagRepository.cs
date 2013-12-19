using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface IQuestionFlagRepository
    {
        QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID);
        QuestionFlag Create();
        void Commit();
        void Delete(QuestionFlag entity);
    }
}
