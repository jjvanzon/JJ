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
        public QuestionDetailViewModel NextQuestion()
        {
            using (var presenter = new QuestionPresenter())
            {
                return presenter.NextQuestion();
            }
        }

        public QuestionDetailViewModel ShowQuestion(int id)
        {
            using (var presenter = new QuestionPresenter())
            {
                return presenter.ShowQuestion(id);
            }
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            using (var presenter = new QuestionPresenter())
            {
                return presenter.ShowAnswer(viewModel);
            }
        }
    }
}
