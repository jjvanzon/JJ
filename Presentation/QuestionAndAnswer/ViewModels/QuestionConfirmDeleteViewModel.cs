using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class QuestionConfirmDeleteViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public int ID { get; set; }
        public string Question { get; set; }
    }
}
