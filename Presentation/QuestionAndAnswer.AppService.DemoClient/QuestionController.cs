using JJ.Presentation.QuestionAndAnswer.AppService.DemoClient.RandomQuestionService;

namespace JJ.Presentation.QuestionAndAnswer.AppService.DemoClient
{
	internal class QuestionController
	{
		private readonly RandomQuestionServiceClient _service = new RandomQuestionServiceClient();

		public RandomQuestionViewModel ShowQuestion() => _service.ShowQuestion();

		public RandomQuestionViewModel ShowAnswer(RandomQuestionViewModel viewModel) => _service.ShowAnswer(viewModel);

		public RandomQuestionViewModel HideAnswer(RandomQuestionViewModel viewModel) => _service.HideAnswer(viewModel);
	}
}