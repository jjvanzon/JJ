//using JJ.Framework.Mvc;
//using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
//using JJ.Presentation.QuestionAndAnswer.Names;
//using JJ.Presentation.QuestionAndAnswer.ViewModels;

//namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
//{
//	public class QuestionIndexViewMapping : ViewMapping<QuestionListViewModel>
//	{
//		public QuestionIndexViewMapping()
//		{
//			MapPresenter(nameof(PresenterNames.QuestionListPresenter), nameof(PresenterActionNames.Show));
//			//MapController(nameof(ControllerNames.Questions), nameof(ActionNames.Index), nameof(ViewNames.Index));
//			MapParameter(nameof(PresenterParameterNames.pageNumber), nameof(ActionParameterNames.page));
//		}

//		//protected override object TryGetRouteValues(QuestionListViewModel viewModel) => new { page = viewModel.Pager.PageNumber };
//	}
//}