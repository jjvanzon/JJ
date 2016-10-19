using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class FlagStatusRepository : RepositoryBase<FlagStatus, int>, IFlagStatusRepository
    {
        public FlagStatusRepository(IContext context)
            : base(context)
        { }
    }
}
