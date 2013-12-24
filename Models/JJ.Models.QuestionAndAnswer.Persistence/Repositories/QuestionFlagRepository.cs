using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class QuestionFlagRepository : IQuestionFlagRepository
    {
        private IContext _context;

        public QuestionFlagRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        public QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID)
        {
            return _context.Query<QuestionFlag>().Where(x => x.Question.ID == questionID)
                                                 .Where(x => x.FlaggedByUser.ID == flaggedByUserID)
                                                 .SingleOrDefault();
        }

        public QuestionFlag Create()
        {
            return _context.Create<QuestionFlag>();
        }

        public void Commit()
        {
            _context.Commit();
        }

        public void Delete(QuestionFlag entity)
        {
            _context.Delete(entity);
        }
    }
}
