using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Framework.Persistence;

namespace JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer, int>
    {
        Answer TryGetByQuestionID(int questionID);
        Answer GetByQuestionID(int questionID);
    }
}
