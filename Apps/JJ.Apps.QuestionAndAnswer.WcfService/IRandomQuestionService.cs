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
    public interface IRandomQuestionService
    {
        [OperationContract]
        RandomQuestionViewModel ShowQuestion();

        [OperationContract]
        RandomQuestionViewModel ShowAnswer(RandomQuestionViewModel viewModel);

        [OperationContract]
        RandomQuestionViewModel HideAnswer(RandomQuestionViewModel viewModel);
    }
}
