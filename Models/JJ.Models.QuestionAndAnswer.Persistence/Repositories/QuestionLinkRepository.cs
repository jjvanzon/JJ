using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class QuestionLinkRepository : IQuestionLinkRepository
    {
        private IContext _context;

        public QuestionLinkRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public QuestionLink Create()
        {
            return _context.Create<QuestionLink>();
        }

        public void Delete(QuestionLink questionLink)
        {
            _context.Delete(questionLink);
        }
    }
}
