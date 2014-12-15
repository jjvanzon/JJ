using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Repositories
{
    public class AnswerRepository : RepositoryBase<Answer, int>, IAnswerRepository
    {
        public AnswerRepository(IContext context)
            : base(context)
        { }
    }
}
