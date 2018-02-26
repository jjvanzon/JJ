using System.Linq;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class Question_SideEffect_SetLastModifiedByUser : ISideEffect
	{
		private readonly Question _question;
		private readonly User _user;
		private readonly EntityStatusManager _statusManager;

		public Question_SideEffect_SetLastModifiedByUser(Question question, User user, EntityStatusManager statusManager)
		{
			_question = question ?? throw new NullException(() => question);
			_user = user ?? throw new NullException(() => user);
			_statusManager = statusManager ?? throw new NullException(() => statusManager);
		}

		public void Execute()
		{
			if (MustSetLastModifiedByUser(_question, _statusManager))
			{
				_question.LastModifiedByUser = _user;
			}
		}

		private bool MustSetLastModifiedByUser(Question entity, EntityStatusManager statusManager)
		{
			return statusManager.IsDirty(entity) ||
				   statusManager.IsNew(entity) ||
				   statusManager.IsDirty(() => entity.QuestionType) ||
				   statusManager.IsDirty(() => entity.Source) ||
				   statusManager.IsDirty(() => entity.QuestionCategories) ||
				   entity.QuestionCategories.Any(x => statusManager.IsDirty(x)) ||
				   statusManager.IsDirty(() => entity.QuestionLinks) ||
				   entity.QuestionLinks.Any(x => statusManager.IsDirty(x)) ||
				   entity.QuestionLinks.Any(x => statusManager.IsNew(x)) ||
				   statusManager.IsDirty(() => entity.QuestionFlags) ||
				   entity.QuestionFlags.Any(x => statusManager.IsDirty(x)) ||
				   entity.QuestionFlags.Any(x => statusManager.IsNew(x));

		}
	}
}
