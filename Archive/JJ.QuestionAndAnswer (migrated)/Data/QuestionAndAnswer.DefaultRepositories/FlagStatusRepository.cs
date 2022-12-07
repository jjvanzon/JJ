using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class FlagStatusRepository : RepositoryBase<FlagStatus, int>, IFlagStatusRepository
    {
        [UsedImplicitly]
        public FlagStatusRepository(IContext context) : base(context) { }
    }
}