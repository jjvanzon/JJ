using JJ.Framework.Persistence;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.QuestionAndAnswer.DefaultRepositories
{
    public class QuestionFlagRepository : RepositoryBase<QuestionFlag, int>, IQuestionFlagRepository
    {
        public QuestionFlagRepository(IContext context)
            : base (context)
        { }

        public QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID)
        {
            return _context.Query<QuestionFlag>().Where(x => x.Question.ID == questionID)
                                                 .Where(x => x.FlaggedByUser.ID == flaggedByUserID)
                                                 .SingleOrDefault();
        }
    }
}
