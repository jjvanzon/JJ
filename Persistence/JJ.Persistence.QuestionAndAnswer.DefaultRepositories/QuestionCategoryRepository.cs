using JJ.Framework.Persistence;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionCategoryRepository : RepositoryBase<QuestionCategory, int>, IQuestionCategoryRepository
    {
        public QuestionCategoryRepository(IContext context)
            : base(context)
        { }
    }
}
