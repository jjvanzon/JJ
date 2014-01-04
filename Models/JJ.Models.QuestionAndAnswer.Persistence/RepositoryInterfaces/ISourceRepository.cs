using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface ISourceRepository : IRepository<Source, int>
    {
        Source TryGetByIdentifier(string identifier);
        Source GetByIdentifier(string identifier);
    }
}
