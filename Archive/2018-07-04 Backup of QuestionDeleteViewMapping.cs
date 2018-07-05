//using JJ.Framework.Mvc;
//using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
//using JJ.Presentation.QuestionAndAnswer.Names;
//using JJ.Presentation.QuestionAndAnswer.ViewModels;

//namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
//{
//	public class QuestionDeleteViewMapping : ViewMapping<QuestionConfirmDeleteViewModel>
//	{
//		public QuestionDeleteViewMapping()
//		{
//			MapPresenter(nameof(PresenterNames.QuestionConfirmDeletePresenter), nameof(PresenterActionNames.Show));
//			//MapController(nameof(ControllerNames.Questions), nameof(ActionNames.Delete), nameof(ViewNames.Delete));
//			MapParameter(nameof(PresenterParameterNames.id), nameof(ActionParameterNames.id));
//		}

//		//protected override object TryGetRouteValues(QuestionConfirmDeleteViewModel viewModel) => new { id = viewModel.ID };
//	}
//}