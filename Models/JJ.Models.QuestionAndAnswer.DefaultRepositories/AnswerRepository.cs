using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.DefaultRepositories
{
    public class AnswerRepository : RepositoryBase<Answer, int>, IAnswerRepository
    {
        public AnswerRepository(IContext context)
            : base(context)
        { }

        public virtual Answer GetByQuestionID(int questionID)
        {
            Answer entity = TryGetByQuestionID(questionID);
            if (entity == null)
            {
                throw new Exception(String.Format("Answer with QuestionID '{0}' not found.", questionID));
            }
            return entity;
        }

        public virtual Answer TryGetByQuestionID(int questionID)
        {
            return _context.Query<Answer>()
                           .Where(x => x.Question.ID == questionID)
                           .SingleOrDefault();
        }
    }
}
