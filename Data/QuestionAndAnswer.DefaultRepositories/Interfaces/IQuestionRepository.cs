using JJ.Framework.Data;
using System.Collections.Generic;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question, int>
    {
        Question TryGetRandomQuestion();
        IList<Question> GetBySourceID(int sourceID);
        /// <summary>
        /// mustFilterByFlagStatusID = false and flagStatusID = null are two 
        /// different things. mustFilterByFlagStatusID = false means all Questions are returned.
        /// flagStatusID = null means only questions without a flagging are returned.
        /// </summary>
        IList<Question> GetByCriteria(bool mustFilterByFlagStatusID, int? flagStatusID);
        IList<Question> GetPage(int firstIndex, int pageSize);
        int Count();

        IList<int> GetQuestionIDsByCategory(Category category);
    }
}
