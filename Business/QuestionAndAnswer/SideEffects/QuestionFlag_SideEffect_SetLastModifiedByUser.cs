using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class QuestionFlag_SideEffect_SetLastModifiedByUser : ISideEffect
	{
		private readonly QuestionFlag _questionFlag;
		private readonly User _user;
		private readonly EntityStatusManager _statusManager;

		public QuestionFlag_SideEffect_SetLastModifiedByUser(QuestionFlag questionFlag, User user, EntityStatusManager statusManager)
		{
			_questionFlag = questionFlag ?? throw new NullException(() => questionFlag);
			_user = user ?? throw new NullException(() => user);
			_statusManager = statusManager ?? throw new NullException(() => statusManager);
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
