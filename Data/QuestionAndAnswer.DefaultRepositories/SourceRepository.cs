using System;
using System.Linq;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;
using JJ.Framework.Exceptions;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class SourceRepository : RepositoryBase<Source, int>, ISourceRepository
    {
        public SourceRepository(IContext context)
            : base(context)
        { }

        public Source GetByIdentifier(string identifier)
        {
            Source source = TryGetByIdentifier(identifier);
            if (source == null)
            {
                throw new NotFoundException<Source>(new { identifier });
            }
            return source;
        }

        public virtual Source TryGetByIdentifier(string identifier)
        {
            return _context.Query<Source>()
                           .Where(x => x.Identifier == identifier)
                           .SingleOrDefault();
        }
    }
}
