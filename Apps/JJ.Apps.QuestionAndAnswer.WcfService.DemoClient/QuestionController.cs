using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.WcfService.DemoClient.QuestionService;
using JJ.Apps.QuestionAndAnswer.WcfService.DemoClient.ResourceService;

namespace JJ.Apps.QuestionAndAnswer.WcfService.DemoClient
{
    public class QuestionController
    {
        private QuestionServiceClient _service = new QuestionServiceClient();

        public QuestionDetailViewModel NextQuestion()
        {
            return _service.NextQuestion();
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
