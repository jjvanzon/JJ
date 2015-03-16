using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Mapping
{
    public class LoginIndexMapping : ViewMapping<LoginViewModel>
    {
        public LoginIndexMapping()
            : base(ViewNames.Index)
        {
            PresenterName = PresenterNames.LoginPresenter;
            PresenterActionName = PresenterActionNames.Show;
            ControllerName = ControllerNames.Login;
            ControllerGetActionName = ActionNames.Index;
        }

        protected override object GetRouteValues(LoginViewModel viewModel)
        {
            return new { ret = GetReturnUrl(viewModel.ReturnAction) };
        }
    }
}