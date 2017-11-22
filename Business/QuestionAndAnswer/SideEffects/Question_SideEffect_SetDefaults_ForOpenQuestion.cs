using JJ.Framework.Business;
using JJ.Framework.Exceptions;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class Question_SideEffect_SetDefaults_ForOpenQuestion : ISideEffect
	{
		private Question _entity;
		private IQuestionTypeRepository _questionTypeRepository;
		private EntityStatusManager _statusManager;

		public Question_SideEffect_SetDefaults_ForOpenQuestion(
			Question entity,
			IQuestionTypeRepository questionTypeRepository,
			EntityStatusManager statusManager)
		{
			if (entity == null) throw new NullException(() => entity);
			if (questionTypeRepository == null) throw new NullException(() => questionTypeRepository);
			if (statusManager == null) throw new NullException(() => statusManager);

			_entity = entity;
			_questionTypeRepository = questionTypeRepository;
			_statusManager = statusManager;
		}

		public void Execute()
		{
			if (_statusManager.IsNew(_entity))
			{
				_entity.IsActive = true;
				_entity.SetQuestionTypeEnum(QuestionTypeEnum.OpenQuestion, _questionTypeRepository);
			}
		}
	}
}
