using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System.ServiceModel;

namespace JJ.Presentation.QuestionAndAnswer.AppService
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
