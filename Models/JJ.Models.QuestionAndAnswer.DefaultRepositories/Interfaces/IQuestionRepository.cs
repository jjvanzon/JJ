using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question, int>
    {
        Question TryGetRandomQuestion();
        IList<Question> GetBySourceID(int sourceID);
        IList<int> GetQuestionIDsByCategory(Category category);
        IList<Question> GetByCriteria(bool mustFilterByFlagStatusID, int? flagStatusID);
        IList<Question> GetPage(int firstIndex, int count);

        int CountAll();
    }
}
