using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface IAnswerRepository : IRepository<Answer, int>
    { }
}
