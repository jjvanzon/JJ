using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces
{
	public interface IQuestionFlagRepository : IRepository<QuestionFlag, int>
	{
		QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID);
	}
}
