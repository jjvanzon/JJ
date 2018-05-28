using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class Question_SideEffect_SetDefaults_ForOpenQuestion : ISideEffect
	{
		private readonly Question _entity;
		private readonly IQuestionTypeRepository _questionTypeRepository;
		private readonly EntityStatusManager _statusManager;

		public Question_SideEffect_SetDefaults_ForOpenQuestion(
			Question entity,
			IQuestionTypeRepository questionTypeRepository,
			EntityStatusManager statusManager)
		{
			_entity = entity ?? throw new NullException(() => entity);
			_questionTypeRepository = questionTypeRepository ?? throw new NullException(() => questionTypeRepository);
			_statusManager = statusManager ?? throw new NullException(() => statusManager);
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
