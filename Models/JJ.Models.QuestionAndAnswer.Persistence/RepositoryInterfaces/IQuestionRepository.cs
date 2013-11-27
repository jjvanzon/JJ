using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface IQuestionRepository
    {
        IEnumerable<Question> GetAll();
        Question Get(int id);
        Question TryGet(int id);
        Question Create();
        void Delete(Question question);
        void Commit();

        Question TryGetRandomQuestion();
        IEnumerable<Question> GetBySourceID(int sourceID);
        int[] GetQuestionIDsByCategory(Category category);
    }
}
