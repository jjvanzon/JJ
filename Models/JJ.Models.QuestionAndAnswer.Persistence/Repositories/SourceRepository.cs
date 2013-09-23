using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class SourceRepository : ISourceRepository
    {
        private IContext _context;

        public SourceRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public Source Get(int id)
        {
            return _context.Get<Source>(id);
        }
    }
}