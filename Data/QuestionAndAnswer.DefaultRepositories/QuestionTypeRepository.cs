using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionTypeRepository : RepositoryBase<QuestionType, int>, IQuestionTypeRepository
    {
        public QuestionTypeRepository(IContext context)
            : base(context)
        { }
    }
}