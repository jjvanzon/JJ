using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
	public class QuestionDetailsViewMapping : ViewMapping<QuestionDetailsViewModel>
	{
		public QuestionDetailsViewMapping()
		{
			MapPresenter(PresenterNames.QuestionDetailsPresenter, PresenterActionNames.Show);
			MapController(ControllerNames.Questions, ActionNames.Details, ViewNames.Details);
			MapParameter(PresenterParameterNames.id, ActionParameterNames.id);
		}

		protected override object GetRouteValues(QuestionDetailsViewModel viewModel)
		{
			return new { id = viewModel.Question.ID };
		}
	}
}
