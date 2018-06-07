using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
using JJ.Business.QuestionAndAnswer.SideEffects;

namespace JJ.Presentation.QuestionAndAnswer.ToEntity
{
	internal static class QuestionViewModelExtensions
	{
		public static Question ToEntity(
			this QuestionViewModel viewModel, 
			IQuestionRepository questionRepository, 
			ISourceRepository sourceRepository,
			IQuestionTypeRepository questionTypeRepository,
			EntityStatusManager entityStatusManager)
		{
			Question question = questionRepository.TryGet(viewModel.ID);

			if (question == null)
			{
				question = questionRepository.Create();

				entityStatusManager.SetIsNew(question);
			}

			question.Text = viewModel.Text;
			question.IsActive = viewModel.IsActive;
			question.Source = sourceRepository.Get(viewModel.Source.ID);
			question.QuestionType = questionTypeRepository.Get(viewModel.Type.ID);

			return question;
		}

		public static Answer ToAnswer(this QuestionViewModel viewModel, IAnswerRepository answerRepository, EntityStatusManager entityStatusManager)
		{
			// TODO: Low prio: Maybe it is better to simply use the question entity as the source of the answer, instead of the repository.
			Answer answer = answerRepository.TryGetByQuestionID(viewModel.ID);
			if (answer == null)
			{
				answer = answerRepository.Create();

				entityStatusManager.SetIsNew(answer);
			}

			ISideEffect sideEffect = new Answer_SideEffect_SetDefaults_ForOpenQuestion(answer, entityStatusManager);
			sideEffect.Execute();

			answer.Text = viewModel.Answer;
			return answer;
		}
	}
}
