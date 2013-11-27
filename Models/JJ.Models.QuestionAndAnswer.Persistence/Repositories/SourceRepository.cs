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

        public Source TryGetByIdentifier(string identifier)
        {
            return _context.Query<Source>().Where(x => x.Identifier == identifier).SingleOrDefault();
        }

        public Source GetByIdentifier(string identifier)
        {
            Source source = TryGetByIdentifier(identifier);
            if (source == null)
            {
                throw new Exception(String.Format("Source with identifier '{0}' not found.", identifier));
            }
            return source;
        }

        public Source Create()
        {
            return _context.Create<Source>();
        }
    }
}