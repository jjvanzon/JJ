using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class FlagStatusRepository : IFlagStatusRepository
    {
        private IContext _context;

        public FlagStatusRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public FlagStatus Get(int id)
        {
            return _context.Get<FlagStatus>(id);
        }
    }
}
