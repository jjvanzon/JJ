using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private IContext _context;

        public AnswerRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public Answer Create()
        {
            return _context.Create<Answer>();
        }

        public void Delete(Answer answer)
        {
            _context.Delete(answer);
        }
    }
}
