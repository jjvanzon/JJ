using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System.Linq;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
	public class QuestionFlagRepository : RepositoryBase<QuestionFlag, int>, IQuestionFlagRepository
	{
		public QuestionFlagRepository(IContext context)
			: base (context)
		{ }

		public virtual QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID)
		{
			return _context.Query<QuestionFlag>()
						   .Where(x => x.Question.ID == questionID)
						   .Where(x => x.FlaggedByUser.ID == flaggedByUserID)
						   .SingleOrDefault();
		}
	}
}
