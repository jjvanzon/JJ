using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.DefaultRepositories
{
    public class SourceRepository : RepositoryBase<Source, int>, ISourceRepository
    {
        public SourceRepository(IContext context)
            : base(context)
        { }

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
    }
}
