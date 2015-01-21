using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Reflection;
using JJ.Framework.Persistence;
using JJ.Framework.Presentation;
using JJ.Framework.Presentation.Mvc;
using JJ.Models.Canonical;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using System.Threading;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    /// <summary>
    /// Provides basic view data and basic actions, such as setting the language and a small login widget.
    /// </summary>
    public abstract class MasterController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cultureName = GetSessionWrapper().CultureName;
            if (!String.IsNullOrEmpty(cultureName))
            {
                CultureInfo cultureInfo = new CultureInfo(GetSessionWrapper().CultureName);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }

            base.OnActionExecuting(filterContext);
        }

        protected SessionWrapper GetSessionWrapper()
        {
            return new SessionWrapper(Session);
        }

        // Login

        protected string TryGetAuthenticatedUserName()
        {
            return GetSessionWrapper().AuthenticatedUserName;
        }

        public void SetAuthenticatedUserName(string authenticatedUserName)
        {
            GetSessionWrapper().AuthenticatedUserName = authenticatedUserName;
        }

        public ActionResult LogOut()
        {
            GetSessionWrapper().AuthenticatedUserName = null;

            return RedirectToAction(ActionNames.Index, ControllerNames.Login);
        }

        // GetActionResult

        private class Names
        {
            public Names(string controllerName, string actionName, string viewName)
            {
                ControllerName = controllerName;
                ActionName = actionName;
                ViewName = viewName;
            }

            public string ControllerName { get; private set; }
            public string ActionName { get; private set; }
            public string ViewName { get; private set; }
        }

        private static IDictionary<Type, Names> _dictionary = new Dictionary<Type, Names>()
        {
            { typeof(CategorySelectorViewModel),        new Names(ControllerNames.CategorySelector, ActionNames.Index,      ViewNames.Index) },
            { typeof(LoginViewModel),                   new Names(ControllerNames.Login,            ActionNames.Index,      ViewNames.Index) },
            { typeof(QuestionConfirmDeleteViewModel),   new Names(ControllerNames.Questions,        ActionNames.Delete,     ViewNames.Delete) },
            { typeof(QuestionDeleteConfirmedViewModel), new Names(ControllerNames.Questions,        null,                   ViewNames.Deleted) },
            { typeof(QuestionDetailsViewModel),         new Names(ControllerNames.Questions,        ActionNames.Details,    ViewNames.Details) },
            { typeof(QuestionListViewModel),            new Names(ControllerNames.Questions,        ActionNames.Index,      ViewNames.Index) },
            { typeof(QuestionNotFoundViewModel),        new Names(ControllerNames.Questions,        null,                   ViewNames.NotFound) },
            { typeof(RandomQuestionViewModel),          new Names(ControllerNames.Questions,        ActionNames.Random,     ViewNames.Random) }
        };

        protected ActionResult GetActionResult(string sourceActionName, object viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            string sourceControllerName = GetControllerName();

            // TODO: Low prio: do the generalized case first?
            // TODO: Low prio: method too long? split up into multiple methods?

            var questionDetailsViewModel = viewModel as QuestionDetailsViewModel;
            if (questionDetailsViewModel != null)
            {
                bool isSameControllerAndAction = String.Equals(sourceControllerName, ControllerNames.Questions) &&
                                                 String.Equals(sourceActionName, ActionNames.Details);
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

            var questionEditViewModel = viewModel as QuestionEditViewModel;
            if (questionEditViewModel != null)
            {
                if (questionEditViewModel.IsNew)
                {
                    bool isSameControllerAndAction = String.Equals(sourceControllerName, ControllerNames.Questions) &&
                                                     String.Equals(sourceActionName, ActionNames.Create);
                    if (isSameControllerAndAction)
                    {
                        ModelState.ClearModelErrors();
                        foreach (ValidationMessage validationMessage in questionEditViewModel.ValidationMessages)
                        {
                            ModelState.AddModelError(validationMessage.PropertyKey, validationMessage.Text);
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
                    bool isSameControllerAndAction = String.Equals(sourceControllerName, ControllerNames.Questions) &&
                                                     String.Equals(sourceActionName, ActionNames.Edit);
                    if (isSameControllerAndAction)
                    {
                        ModelState.ClearModelErrors();
                        foreach (ValidationMessage validationMessage in questionEditViewModel.ValidationMessages)
                        {
                            ModelState.AddModelError(validationMessage.PropertyKey, validationMessage.Text);
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

            var questionConfirmDeleteViewModel = viewModel as QuestionConfirmDeleteViewModel;
            if (questionConfirmDeleteViewModel != null)
            {
                bool isSameControllerAndAction = String.Equals(sourceControllerName, ControllerNames.Questions) &&
                                                 String.Equals(sourceActionName, ActionNames.Delete);
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

            Names names;
            if (_dictionary.TryGetValue(viewModel.GetType(), out names))
            {
                bool hasActionName = !String.IsNullOrEmpty(names.ActionName);
                if (!hasActionName)
                {
                    return View(names.ViewName, viewModel);
                }

                bool isSameControllerAndAction = String.Equals(names.ControllerName, sourceControllerName) &&
                                                 String.Equals(names.ActionName, sourceActionName);
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

            throw new UnexpectedViewModelTypeException(viewModel);
        }

        private string GetControllerName()
        {
            string controllerName = (string)ControllerContext.RequestContext.RouteData.Values["controller"];
            return controllerName;
        }
    }
}
