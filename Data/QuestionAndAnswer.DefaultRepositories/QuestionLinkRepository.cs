using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionLinkRepository : RepositoryBase<QuestionLink, int>, IQuestionLinkRepository
    {
        public QuestionLinkRepository(IContext context)
            : base(context)
        { }
    }
}
