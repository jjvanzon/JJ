using JJ.Framework.Presentation;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System.Collections.Generic;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
	public sealed class QuestionListViewModel
	{
		public LoginPartialViewModel Login { get; set; }
		public IList<QuestionViewModel> List { get; set; }
		public PagerViewModel Pager { get; set; }
	}
}
