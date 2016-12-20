using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionCategoryRepository : RepositoryBase<QuestionCategory, int>, IQuestionCategoryRepository
    {
        public QuestionCategoryRepository(IContext context)
            : base(context)
        { }
    }
}
