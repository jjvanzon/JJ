using JJ.Apps.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.WcfService
{
    [ServiceContract]
    public interface IQuestionService
    {
        [OperationContract]
        QuestionDetailViewModel NextQuestion();

        [OperationContract]
        QuestionDetailViewModel ShowQuestion(int id);

        [OperationContract]
        QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel);

        [OperationContract]
        QuestionDetailViewModel HideAnswer(QuestionDetailViewModel viewModel);
    }
}
