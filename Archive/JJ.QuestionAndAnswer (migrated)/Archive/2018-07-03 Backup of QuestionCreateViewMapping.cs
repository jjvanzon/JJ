//using System.Collections.Generic;
//using JJ.Framework.Mvc;
//using JJ.Presentation.QuestionAndAnswer.Names;
//using JJ.Presentation.QuestionAndAnswer.ViewModels;

//namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
//{
//	public class QuestionCreateViewMapping : ViewMapping<QuestionEditViewModel>
//	{
//		public QuestionCreateViewMapping()
//		{
//			MapPresenter(nameof(PresenterNames.QuestionEditPresenter), nameof(PresenterActionNames.Create));
//			//MapController(nameof(ControllerNames.Questions), nameof(ActionNames.Create), nameof(ViewNames.Edit));
//		}

//		//protected override bool Predicate(QuestionEditViewModel viewModel) => viewModel.IsNew;

//		protected override object TryGetRouteValues(QuestionEditViewModel viewModel)
//			=> new
//			{
//				//id = viewModel.Question.ID,
//				//ret = TryGetReturnUrl(viewModel.ReturnAction)
//			};

//		//protected override ICollection<string> GetValidationMesssages(QuestionEditViewModel viewModel) => viewModel.ValidationMessages;
//	}
//}