using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionLinkRepository : RepositoryBase<QuestionLink, int>, IQuestionLinkRepository
    {
        [UsedImplicitly]
        public QuestionLinkRepository(IContext context) : base(context) { }
    }
}