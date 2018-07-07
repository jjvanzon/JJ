using JJ.Business.QuestionAndAnswer.Helpers;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class Answer_SideEffect_SetDefaults_ForOpenQuestion : ISideEffect
	{
		private readonly Answer _entity;
		private readonly EntityStatusManager _statusManager;

		public Answer_SideEffect_SetDefaults_ForOpenQuestion(Answer entity, EntityStatusManager statusManager)
		{
			_entity = entity ?? throw new NullException(() => entity);
			_statusManager = statusManager ?? throw new NullException(() => statusManager);
		}

		public void Execute()
		{
			if (_statusManager.IsNew(_entity))
			{
				_entity.IsCorrectAnswer = true;
			}
		}
	}
}
