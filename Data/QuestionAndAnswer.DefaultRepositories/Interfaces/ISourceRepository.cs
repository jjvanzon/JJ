using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces
{
    public interface ISourceRepository : IRepository<Source, int>
    {
        Source GetByIdentifier(string identifier);
        Source TryGetByIdentifier(string identifier);
    }
}
