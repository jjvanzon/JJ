using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionCategoryRepository : RepositoryBase<QuestionCategory, int>, IQuestionCategoryRepository
    {
        [UsedImplicitly]
        public QuestionCategoryRepository(IContext context) : base(context) { }
    }
}