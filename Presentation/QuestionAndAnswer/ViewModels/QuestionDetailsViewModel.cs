using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
	public sealed class QuestionDetailsViewModel
	{
		public LoginPartialViewModel Login { get; set; }
		public QuestionViewModel Question { get; set; }
	}
}
