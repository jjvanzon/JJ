//using JJ.Framework.Mvc;
//using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
//using JJ.Presentation.QuestionAndAnswer.Names;
//using JJ.Presentation.QuestionAndAnswer.ViewModels;

//namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
//{
//	public class QuestionDetailsViewMapping : ViewMapping<QuestionDetailsViewModel>
//	{
//		public QuestionDetailsViewMapping()
//		{
//			MapPresenter(nameof(PresenterNames.QuestionDetailsPresenter), nameof(PresenterActionNames.Show));
//			//MapController(nameof(ControllerNames.Questions), nameof(ActionNames.Details), nameof(ViewNames.Details));
//			MapParameter(nameof(PresenterParameterNames.id), nameof(ActionParameterNames.id));
//		}

//		//protected override object TryGetRouteValues(QuestionDetailsViewModel viewModel) => new { id = viewModel.Question.ID };
//	}
//}
