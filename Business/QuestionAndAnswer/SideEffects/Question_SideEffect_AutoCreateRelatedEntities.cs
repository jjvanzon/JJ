using System.Linq;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class Question_SideEffect_AutoCreateRelatedEntities : ISideEffect
	{
		private readonly Question _question;
		private readonly IAnswerRepository _answerRepository;
		private readonly EntityStatusManager _entityStatusManager;

		public Question_SideEffect_AutoCreateRelatedEntities(Question question, IAnswerRepository answerRepository, EntityStatusManager entityStatusManager)
		{
			if (question == null) throw new NullException(() => question);
			if (answerRepository == null) throw new NullException(() => answerRepository);
			if (entityStatusManager == null) throw new NullException(() => entityStatusManager);

			_question = question;
			_answerRepository = answerRepository;
			_entityStatusManager = entityStatusManager;
		}

		public void Execute()
		{
			if (!_question.Answers.Any())
			{
				Answer answer = _answerRepository.Create();
				_entityStatusManager.SetIsNew(answer);
				answer.LinkTo(_question);
			}
		}
	}
}
