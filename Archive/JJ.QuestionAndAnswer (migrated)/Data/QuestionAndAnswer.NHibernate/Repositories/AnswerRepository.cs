using JetBrains.Annotations;
using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
    public class AnswerRepository : DefaultRepositories.AnswerRepository
    {
        private new readonly NHibernateContext _context;

        [UsedImplicitly]
        public AnswerRepository(IContext context) : base(context) => _context = (NHibernateContext)context;

        public override Answer TryGetByQuestionID(int questionID)
            => _context.Session.QueryOver<Answer>()
                       .Where(x => x.Question.ID == questionID)
                       .SingleOrDefault();
    }
}