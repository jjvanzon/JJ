using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.Repositories
{
    public class QuestionTypeRepository : RepositoryBase<QuestionType, int>, IQuestionTypeRepository
    {
        public QuestionTypeRepository(IContext context)
            : base(context)
        { }
    }
}