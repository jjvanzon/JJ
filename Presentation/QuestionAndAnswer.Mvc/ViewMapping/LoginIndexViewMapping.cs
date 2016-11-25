using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class LoginIndexViewMapping : ViewMapping<LoginViewModel>
    {
        public LoginIndexViewMapping()
        {
            MapPresenter(PresenterNames.LoginPresenter, PresenterActionNames.Show);
            MapController(ControllerNames.Login, ActionNames.Index, ViewNames.Index);
        }

        protected override object GetRouteValues(LoginViewModel viewModel)
        {
            return new { ret = TryGetReturnUrl(viewModel.ReturnAction) };
        }
    }
}