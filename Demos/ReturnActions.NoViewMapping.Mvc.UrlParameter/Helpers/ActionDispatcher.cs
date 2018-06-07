﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using JJ.Demos.ReturnActions.Mvc.Names;
using JJ.Demos.ReturnActions.NoViewMapping.Mvc.UrlParameter.Names;
using JJ.Demos.ReturnActions.NoViewMapping.ViewModels;
using JJ.Demos.ReturnActions.ViewModels;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Exceptions.Comparative;
using JJ.Framework.Mvc;

namespace JJ.Demos.ReturnActions.NoViewMapping.Mvc.UrlParameter.Helpers
{
    // TODO: Use ActionDispatcherBase from JJ.Framework.Mvc.
    public static class ActionDispatcher
    {
        private static readonly Dictionary<Type, (string controllerName, string httpGetActionName, string viewName)>
            _viewModelTypeToActionTupleDictionary =
                new Dictionary<Type, (string controllerName, string httpGetActionName, string viewName)>
                {
                    [typeof(ListViewModel)] = (nameof(ControllerNames.Demo), nameof(ActionNames.Index), nameof(ViewNames.Index)),
                    [typeof(DetailsViewModel)] = (nameof(ControllerNames.Demo), nameof(ActionNames.Details), nameof(ViewNames.Details)),
                    [typeof(EditViewModel)] = (nameof(ControllerNames.Demo), nameof(ActionNames.Edit), nameof(ViewNames.Edit)),
                    [typeof(LoginViewModel)] = (nameof(ControllerNames.Login), nameof(ActionNames.Index), nameof(ViewNames.Index))
                };

        public static ActionResult Dispatch(
            Controller sourceController,
            object viewModel,
            [CallerMemberName] string sourceActionName = "")
        {
            if (sourceController == null) throw new NullException(() => sourceController);
            if (string.IsNullOrEmpty(sourceActionName)) throw new NullOrEmptyException(() => sourceActionName);
            if (viewModel == null) throw new NullException(() => viewModel);

            Type viewModelType = viewModel.GetType();
            if (!_viewModelTypeToActionTupleDictionary.TryGetValue(viewModelType, out (string, string, string) tuple))
            {
                throw new NotContainsException(nameof(_viewModelTypeToActionTupleDictionary), () => viewModelType);
            }

            (string destControllerName, string destHttpGetActionName, string destViewName) = tuple;

            var sourceControllerAccessor = new ControllerAccessor(sourceController);
            string sourceControllerName = sourceController.GetControllerName();

            bool hasActionName = !string.IsNullOrEmpty(destHttpGetActionName);
            if (!hasActionName)
            {
                return sourceControllerAccessor.View(destViewName, viewModel);
            }

            bool isSameControllerAndAction = string.Equals(destControllerName, sourceControllerName) &&
                                             string.Equals(destHttpGetActionName, sourceActionName);

            bool mustReturnView = isSameControllerAndAction;
            if (mustReturnView)
            {
                sourceController.ModelState.ClearModelErrors();

                foreach (string message in GetValidationMesssages(viewModel))
                {
                    sourceController.ModelState.AddModelError(nameof(message), message);
                }

                return sourceControllerAccessor.View(destViewName, viewModel);
            }

            sourceController.TempData[nameof(TempDataKeys.ViewModel)] = viewModel;

            object parameters = TryGetRouteValues(viewModel);

            return sourceControllerAccessor.RedirectToAction(destHttpGetActionName, destControllerName, parameters);
        }

        private static object TryGetRouteValues(object viewModel)
        {
            switch (viewModel)
            {
                case DetailsViewModel viewModel2: return new { id = viewModel2.Entity.ID };
                case EditViewModel viewModel2: return new { id = viewModel2.Entity.ID };
            }

            return null;
        }

        private static IList<string> GetValidationMesssages(object viewModel) => new string[0];
    }
}