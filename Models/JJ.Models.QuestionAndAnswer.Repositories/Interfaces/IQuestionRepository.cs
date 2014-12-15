using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Repositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question, int>
    {
        Question TryGetRandomQuestion();
        IEnumerable<Question> GetBySourceID(int sourceID);
        IList<int> GetQuestionIDsByCategory(Category category);
        IEnumerable<Question> GetByCriteria(bool mustFilterByFlagStatusID, int? flagStatusID);
    }
}
