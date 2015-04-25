using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionCategoryRepository : RepositoryBase<QuestionCategory, int>, IQuestionCategoryRepository
    {
        public QuestionCategoryRepository(IContext context)
            : base(context)
        { }
    }
}
