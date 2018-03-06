using System.Linq;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.SideEffects
{
	public class Question_SideEffect_AutoCreateRelatedEntities : ISideEffect
	{
		private readonly Question _question;
		private readonly IAnswerRepository _answerRepository;
		private readonly EntityStatusManager _entityStatusManager;

		public Question_SideEffect_AutoCreateRelatedEntities(
			Question question,
			IAnswerRepository answerRepository,
			EntityStatusManager entityStatusManager)
		{
			_question = question ?? throw new NullException(() => question);
			_answerRepository = answerRepository ?? throw new NullException(() => answerRepository);
			_entityStatusManager = entityStatusManager ?? throw new NullException(() => entityStatusManager);
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