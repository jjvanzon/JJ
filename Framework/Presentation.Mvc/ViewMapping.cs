﻿using JJ.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Framework.Presentation.Mvc
{
    public abstract class ViewMapping<TViewModel> : IViewMapping
    {
        public string PresenterName { get; protected set; }
        public string PresenterActionName { get; protected set; }
        public string ControllerName { get; protected set; }
        public string ControllerGetActionName { get; protected set; }
        public string ViewName { get; protected set; }

        // TODO: Is this enough encapsulation?
        public IList<ActionParameterMapping> ParameterMappings { get; private set; }

        public ViewMapping()
        {
            ParameterMappings = new List<ActionParameterMapping>();
        }

        /// <summary> nullable, base method does nothing </summary>
        protected virtual object GetRouteValues(TViewModel viewModel)
        {
            return null;
        }

        /// <summary> not nullable </summary>
        protected virtual ICollection<KeyValuePair<string, string>> GetValidationMesssages(TViewModel viewModel)
        {
            return new KeyValuePair<string, string>[0];
        }

        protected virtual bool Predicate(TViewModel viewModel)
        {
            return true;
        }

        /// <summary>
        /// Takes presenter action info and converts it to an MVC url.
        /// </summary>
        protected string TryGetReturnUrl(ActionInfo actionInfo)
        {
            if (actionInfo == null)
            {
                return null;
            }

            return ActionDispatcher.GetUrl(actionInfo);
        }

        /// <summary>
        /// Syntactic sugar for assigning PresenterName and PresenterActionName.
        /// </summary>
        protected void MapPresenter(string presenterName, string presenterActionName)
        {
            if (String.IsNullOrEmpty(presenterName)) throw new NullOrEmptyException(() => presenterName);
            if (String.IsNullOrEmpty(presenterActionName)) throw new NullOrEmptyException(() => presenterActionName);

            PresenterName = presenterName;
            PresenterActionName = presenterActionName;
        }

        /// <summary>
        /// Syntactic sugar for assigning ControllerName, ControllerGetActionName and ViewName.
        /// </summary>
        protected void MapController(string controllerName, string controllerGetActionName, string viewName)
        {
            if (String.IsNullOrEmpty(controllerName)) throw new NullOrEmptyException(() => controllerName);
            if (String.IsNullOrEmpty(controllerGetActionName)) throw new NullOrEmptyException(() => controllerGetActionName);
            if (String.IsNullOrEmpty(viewName)) throw new NullOrEmptyException(() => viewName);

            ControllerName = controllerName;
            ControllerGetActionName = controllerGetActionName;
            ViewName = viewName;
        }

        /// <summary>
        /// Syntactic sugar for assigning ControllerName and ViewName.
        /// </summary>
        public void MapController(string controllerName, string viewName)
        {
            if (String.IsNullOrEmpty(viewName)) throw new NullOrEmptyException(() => viewName);

            ControllerName = controllerName;
            ViewName = viewName;
        }

        public void MapParameter(string presenterActionParameter, string controllerActionParameter)
        {
            ParameterMappings.Add(new ActionParameterMapping(presenterActionParameter, controllerActionParameter));
        }

        // IViewMapping

        string IViewMapping.ViewName
        {
            get { return ViewName; }
        }

        string IViewMapping.PresenterName
        {
            get { return PresenterName; }
        }

        string IViewMapping.PresenterActionName
        {
            get { return PresenterActionName; }
        }

        string IViewMapping.ControllerName
        {
            get { return ControllerName; }
        }

        string IViewMapping.ControllerGetActionName
        {
            get { return ControllerGetActionName; }
        }

        object IViewMapping.GetRouteValues(object viewModel)
        {
            return GetRouteValues((TViewModel)viewModel);
        }

        ICollection<KeyValuePair<string, string>> IViewMapping.GetValidationMesssages(object viewModel)
        {
            return GetValidationMesssages((TViewModel)viewModel);
        }

        bool IViewMapping.Predicate(object viewModel)
        {
            return Predicate((TViewModel)viewModel);
        }

        Type IViewMapping.ViewModelType
        {
            get { return typeof(TViewModel); }
        }
    }
}