﻿using System.Web.Mvc;
using JJ.Demos.ReturnActions.MvcUrlParameterViewMappings.Names;
using JJ.Demos.ReturnActions.Presenters;
using JJ.Demos.ReturnActions.ViewModels;
using JJ.Framework.Presentation;
using ActionDispatcher = JJ.Framework.Mvc.ActionDispatcher;
// ReSharper disable AccessToStaticMemberViaDerivedType

namespace JJ.Demos.ReturnActions.MvcUrlParameterViewMappings.Controllers
{
	public class LoginController : MasterController
	{
		public ActionResult Index(string ret = null)
		{
			if (!TempData.TryGetValue(ActionDispatcher.TempDataKey, out object viewModel))
			{
				var presenter = new LoginPresenter();
				ActionInfo returnAction = ActionDispatcher.TryGetActionInfo(ret);
				viewModel = presenter.Show(returnAction);
			}

			return ActionDispatcher.Dispatch(this, nameof(ActionNames.Index), viewModel);
		}

		[HttpPost]
		public ActionResult Index(LoginViewModel viewModel, string ret = null)
		{
			var presenter = new LoginPresenter();
			viewModel.ReturnAction = ActionDispatcher.TryGetActionInfo(ret);
			object viewModel2 = presenter.Login(viewModel);

			// TODO: This is dirty.
			if (!(viewModel2 is LoginViewModel))
			{
				SetAuthenticatedUserName(viewModel.UserName);
			}

			return ActionDispatcher.Dispatch(this, nameof(ActionNames.Index), viewModel2);
		}
	}
}