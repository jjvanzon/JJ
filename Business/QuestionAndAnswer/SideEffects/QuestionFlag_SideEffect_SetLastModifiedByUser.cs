using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class QuestionFlag_SideEffect_SetLastModifiedByUser : ISideEffect
	{
		private readonly QuestionFlag _questionFlag;
		private readonly User _user;
		private readonly EntityStatusManager _statusManager;

		public QuestionFlag_SideEffect_SetLastModifiedByUser(QuestionFlag questionFlag, User user, EntityStatusManager statusManager)
		{
			if (questionFlag == null) throw new NullException(() => questionFlag);
			if (user == null) throw new NullException(() => user);
			if (statusManager == null) throw new NullException(() => statusManager);

			_questionFlag = questionFlag;
			_user = user;
			_statusManager = statusManager;
		}

		public void Execute()
		{
			if (MustSetLastModifiedByUser(_questionFlag, _statusManager))
			{
				_questionFlag.LastModifiedByUser = _user;
			}
		}

		private bool MustSetLastModifiedByUser(QuestionFlag questionFlag, EntityStatusManager statusManager)
		{
			return statusManager.IsDirty(questionFlag) ||
				   statusManager.IsNew(questionFlag);
		}
	}
}
