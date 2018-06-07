using System.Collections.Generic;
using JJ.Framework.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
	public class QuestionEditViewMapping : ViewMapping<QuestionEditViewModel>
	{
		public QuestionEditViewMapping()
		{
			MapPresenter(nameof(PresenterNames.QuestionEditPresenter), nameof(PresenterActionNames.Edit));
			MapController(nameof(ControllerNames.Questions), nameof(ActionNames.Edit), nameof(ViewNames.Edit));
			MapParameter(nameof(PresenterParameterNames.id), nameof(ActionParameterNames.id));
		}

		protected override bool Predicate(QuestionEditViewModel viewModel) => !viewModel.IsNew;

		protected override object TryGetRouteValues(QuestionEditViewModel viewModel)
			=> new
			{
				id = viewModel.Question.ID,
				ret = TryGetReturnUrl(viewModel.ReturnAction)
			};

		protected override ICollection<string> GetValidationMesssages(QuestionEditViewModel viewModel) => viewModel.ValidationMessages;
	}
}