using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class AnswerRepository : RepositoryBase<Answer, int>, IAnswerRepository
    {
        public AnswerRepository(IContext context)
            : base(context)
        { }
    }
}
