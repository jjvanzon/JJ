using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionTypeRepository : RepositoryBase<QuestionType, int>, IQuestionTypeRepository
    {
        [UsedImplicitly]
        public QuestionTypeRepository(IContext context) : base(context) { }
    }
}