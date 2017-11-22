using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;


namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
	public class FlagStatusRepository : RepositoryBase<FlagStatus, int>, IFlagStatusRepository
	{
		public FlagStatusRepository(IContext context)
			: base(context)
		{ }
	}
}
