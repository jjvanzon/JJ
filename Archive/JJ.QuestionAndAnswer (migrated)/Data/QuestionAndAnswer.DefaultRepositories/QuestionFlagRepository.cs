using System.Linq;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionFlagRepository : RepositoryBase<QuestionFlag, int>, IQuestionFlagRepository
    {
        [UsedImplicitly]
        public QuestionFlagRepository(IContext context) : base(context) { }

        public virtual QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID)
            => _context.Query<QuestionFlag>()
                       .Where(x => x.Question.ID == questionID)
                       .Where(x => x.FlaggedByUser.ID == flaggedByUserID)
                       .SingleOrDefault();
    }
}