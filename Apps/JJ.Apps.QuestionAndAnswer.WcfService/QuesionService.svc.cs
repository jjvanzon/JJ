using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;

namespace JJ.Apps.QuestionAndAnswer.WcfService
{
    public class QuesionService : IQuestionService
    {
        public QuestionDetailViewModel ShowQuestion()
        {
            using (var presenter = new QuestionPresenter())
            {
                return presenter.ShowQuestion();
            }
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            using (var presenter = new QuestionPresenter())
            {
                return presenter.ShowAnswer(viewModel);
            }
        }

        public QuestionDetailViewModel HideAnswer(QuestionDetailViewModel viewModel)
        {
            using (var presenter = new QuestionPresenter())
            {
                return presenter.HideAnswer(viewModel);
            }
        }
    }
}
