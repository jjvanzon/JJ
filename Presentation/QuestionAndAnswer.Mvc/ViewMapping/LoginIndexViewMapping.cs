using JJ.Framework.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
	public class LoginIndexViewMapping : ViewMapping<LoginViewModel>
	{
		public LoginIndexViewMapping()
		{
			MapPresenter(nameof(PresenterNames.LoginPresenter), nameof(PresenterActionNames.Show));
			MapController(nameof(ControllerNames.Login), nameof(ActionNames.Index), nameof(ViewNames.Index));
		}

		protected override object TryGetRouteValues(LoginViewModel viewModel) => new { ret = TryGetReturnUrl(viewModel.ReturnAction) };
	}
}