using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.AppService.DemoClient.QuestionService;
using JJ.Apps.QuestionAndAnswer.AppService.DemoClient.ResourceService;

namespace JJ.Apps.QuestionAndAnswer.AppService.DemoClient
{
    public class QuestionController
    {
        private QuestionServiceClient _service = new QuestionServiceClient();

        public QuestionDetailViewModel ShowQuestion()
        {
            return _service.ShowQuestion();
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            return _service.ShowAnswer(viewModel);
        }

        public QuestionDetailViewModel HideAnswer(QuestionDetailViewModel viewModel)
        {
            return _service.HideAnswer(viewModel);
        }
    }
}
