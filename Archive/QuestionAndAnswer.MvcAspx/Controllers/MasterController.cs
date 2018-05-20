using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Mvc;
using JJ.Framework.Presentation;
using JJ.Framework.Web;
using JJ.Presentation.QuestionAndAnswer.MvcAspx.Helpers;
using JJ.Presentation.QuestionAndAnswer.MvcAspx.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.MvcAspx.Controllers
{
	public abstract class MasterController : Controller
	{
		private class Names
		{
			public Names(string controllerName, string actionName, string viewName)
			{
				ControllerName = controllerName;
				ActionName = actionName;
				ViewName = viewName;
			}

			public string ControllerName { get; }
			public string ActionName { get; }
			public string ViewName { get; }
		}

		private const string DEFAULT_CULTURE_NAME = "en-US";

		protected SessionWrapper SessionWrapper { get; private set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			SessionWrapper = new SessionWrapper(Session);

			CultureWebHelper.SetThreadCultureByHttpHeaderOrCookie(ControllerContext.HttpContext, DEFAULT_CULTURE_NAME);

			base.OnActionExecuting(filterContext);
		}

		// Login

		protected string TryGetAuthenticatedUserName()
		{
			return SessionWrapper.AuthenticatedUserName;
		}

		public void SetAuthenticatedUserName(string authenticatedUserName)
		{
			SessionWrapper.AuthenticatedUserName = authenticatedUserName;
		}

		// GetActionResult

		protected ActionResult GetActionResult(string sourceActionName, object viewModel)
		{
			if (viewModel == null) throw new NullException(() => viewModel);

			ActionResult actionResult;

			actionResult = TryGetActionResultFromDictionary(sourceActionName, viewModel);
			if (actionResult != null)
			{
				return actionResult;
			}

			actionResult = TryGetQuestionDetailsActionResult(sourceActionName, viewModel);
			if (actionResult != null)
			{
				return actionResult;
			}

			actionResult = TryGetQuestionEditActionResult(sourceActionName, viewModel);
			if (actionResult != null)
			{
				return actionResult;
			}

			actionResult = TryGetQuestionConfirmDeleteActionResult(sourceActionName, viewModel);
			if (actionResult != null)
			{
				return actionResult;
			}

			throw new UnexpectedViewModelTypeException(viewModel);
		}

		private static readonly Dictionary<Type, Names> _dictionary = new Dictionary<Type, Names>
		{
			{ typeof(CategorySelectorViewModel), new Names(ControllerNames.CategorySelector, ActionNames.Index, ViewNames.Index) },
			{ typeof(LoginViewModel), new Names(ControllerNames.Login, ActionNames.Index, ViewNames.Index) },
			{ typeof(QuestionDeleteConfirmedViewModel), new Names(ControllerNames.Questions, null, ViewNames.Deleted) },
			{ typeof(QuestionListViewModel), new Names(ControllerNames.Questions, ActionNames.Index, ViewNames.Index) },
			{ typeof(QuestionNotFoundViewModel), new Names(ControllerNames.Questions, null, ViewNames.NotFound) },
			{ typeof(RandomQuestionViewModel), new Names(ControllerNames.Questions, ActionNames.Random, ViewNames.Random) },
			{ typeof(NotAuthorizedViewModel), new Names(null, null, ViewNames.NotAuthorized) }
		};

		private ActionResult TryGetActionResultFromDictionary(string sourceActionName, object viewModel)
		{
			if (_dictionary.TryGetValue(viewModel.GetType(), out Names names))
			{
				bool hasActionName = !string.IsNullOrEmpty(names.ActionName);
				if (!hasActionName)
				{
					return View(names.ViewName, viewModel);
				}

				bool isSameControllerAndAction = string.Equals(names.ControllerName, GetControllerName()) &&
												 string.Equals(names.ActionName, sourceActionName);
				if (isSameControllerAndAction)
				{
					return View(names.ViewName, viewModel);
				}
				else
				{
					TempData[TempDataKeys.ViewModel] = viewModel;
					return RedirectToAction(names.ActionName, names.ControllerName);
				}
			}

			return null;
		}

		private ActionResult TryGetQuestionEditActionResult(string sourceActionName, object viewModel)
		{
			if (!(viewModel is QuestionEditViewModel questionEditViewModel))
			{
				return null;
			}

			if (questionEditViewModel.IsNew)
			{
				bool isSameControllerAndAction = string.Equals(GetControllerName(), ControllerNames.Questions) &&
												 string.Equals(sourceActionName, ActionNames.Create);
				if (isSameControllerAndAction)
				{
					ModelState.ClearModelErrors();
					foreach (string message in questionEditViewModel.ValidationMessages)
					{
						ModelState.AddModelError(nameof(message), message);
					}

					return View(ViewNames.Edit, viewModel);
				}
				else
				{
					TempData[TempDataKeys.ViewModel] = viewModel;
					return RedirectToAction(ActionNames.Create, ControllerNames.Questions);
				}
			}
			else
			{
				bool isSameControllerAndAction = string.Equals(GetControllerName(), ControllerNames.Questions) &&
												 string.Equals(sourceActionName, ActionNames.Edit);
				if (isSameControllerAndAction)
				{
					ModelState.ClearModelErrors();
					foreach (string message in questionEditViewModel.ValidationMessages)
					{
						ModelState.AddModelError(nameof(message), message);
					}

					return View(ViewNames.Edit, viewModel);
				}
				else
				{
					TempData[TempDataKeys.ViewModel] = viewModel;
					return RedirectToAction(ActionNames.Edit, ControllerNames.Questions, new { id = questionEditViewModel.Question.ID });
				}
			}
		}

		private ActionResult TryGetQuestionDetailsActionResult(string sourceActionName, object viewModel)
		{
			if (viewModel is QuestionDetailsViewModel questionDetailsViewModel)
			{
				bool isSameControllerAndAction = string.Equals(GetControllerName(), ControllerNames.Questions) &&
												 string.Equals(sourceActionName, ActionNames.Details);
				if (isSameControllerAndAction)
				{
					return View(viewModel);
				}
				else
				{
					TempData[TempDataKeys.ViewModel] = viewModel;
					return RedirectToAction(ActionNames.Details, new { id = questionDetailsViewModel.Question.ID });
				}
			}

			return null;
		}

		private ActionResult TryGetQuestionConfirmDeleteActionResult(string sourceActionName, object viewModel)
		{
			if (viewModel is QuestionConfirmDeleteViewModel questionConfirmDeleteViewModel)
			{
				bool isSameControllerAndAction = string.Equals(GetControllerName(), ControllerNames.Questions) &&
												 string.Equals(sourceActionName, ActionNames.Delete);
				if (isSameControllerAndAction)
				{
					return View(viewModel);
				}
				else
				{
					TempData[TempDataKeys.ViewModel] = viewModel;
					return RedirectToAction(ActionNames.Delete, ControllerNames.Questions, new { id = questionConfirmDeleteViewModel.ID });
				}
			}

			return null;
		}

		private string GetControllerName()
		{
			string controllerName = (string)ControllerContext.RequestContext.RouteData.Values["controller"];
			return controllerName;
		}
	}
}
