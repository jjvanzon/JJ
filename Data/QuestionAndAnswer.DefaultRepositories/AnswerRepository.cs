using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Linq;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
	public class AnswerRepository : RepositoryBase<Answer, int>, IAnswerRepository
	{
		public AnswerRepository(IContext context)
			: base(context)
		{ }

		public Answer GetByQuestionID(int questionID)
		{
			Answer entity = TryGetByQuestionID(questionID);
			if (entity == null)
			{
				throw new Exception(string.Format("Answer with QuestionID '{0}' not found.", questionID));
			}
			return entity;
		}

		public virtual Answer TryGetByQuestionID(int questionID)
		{
			return _context.Query<Answer>()
						   .Where(x => x.Question.ID == questionID)
						   .SingleOrDefault();
		}
	}
}
