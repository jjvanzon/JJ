using JJ.Framework.Business;
using JJ.Framework.Exceptions;
using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class QuestionFlag_SideEffect_SetLastModifiedByUser : ISideEffect
	{
		private QuestionFlag _questionFlag;
		private User _user;
		private EntityStatusManager _statusManager;

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
