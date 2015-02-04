using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionLinkRepository : RepositoryBase<QuestionLink, int>, IQuestionLinkRepository
    {
        public QuestionLinkRepository(IContext context)
            : base(context)
        { }
    }
}
