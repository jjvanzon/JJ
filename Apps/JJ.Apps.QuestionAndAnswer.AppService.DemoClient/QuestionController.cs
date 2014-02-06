using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.AppService.DemoClient.ResourceService;
using JJ.Apps.QuestionAndAnswer.AppService.DemoClient.RandomQuestionService;

namespace JJ.Apps.QuestionAndAnswer.AppService.DemoClient
{
    public class QuestionController
    {
        private RandomQuestionServiceClient _service = new RandomQuestionServiceClient();

        public RandomQuestionViewModel ShowQuestion()
        {
            return _service.ShowQuestion();
        }

        public RandomQuestionViewModel ShowAnswer(RandomQuestionViewModel viewModel)
        {
            return _service.ShowAnswer(viewModel);
        }

        public RandomQuestionViewModel HideAnswer(RandomQuestionViewModel viewModel)
        {
            return _service.HideAnswer(viewModel);
        }
    }
}
