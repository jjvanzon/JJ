using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.Repositories.Interfaces
{
    public interface ISourceRepository : IRepository<Source, int>
    {
        Source TryGetByIdentifier(string identifier);
        Source GetByIdentifier(string identifier);
    }
}
