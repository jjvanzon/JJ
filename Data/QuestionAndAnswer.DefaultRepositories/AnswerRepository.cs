using System.Linq;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;
using JJ.Framework.Exceptions.Aggregates;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class AnswerRepository : RepositoryBase<Answer, int>, IAnswerRepository
    {
        [UsedImplicitly]
        public AnswerRepository(IContext context) : base(context) { }

        public Answer GetByQuestionID(int questionID)
        {
            Answer entity = TryGetByQuestionID(questionID);
            if (entity == null)
            {
                throw new NotFoundException<Answer>(new { questionID });
            }

            return entity;
        }

        public virtual Answer TryGetByQuestionID(int questionID)
            => _context.Query<Answer>()
                       .Where(x => x.Question.ID == questionID)
                       .SingleOrDefault();
    }
}