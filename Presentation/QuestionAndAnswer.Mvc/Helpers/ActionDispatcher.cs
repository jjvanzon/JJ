using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using JJ.Framework.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Helpers
{
    internal class ActionDispatcher : ActionDispatcherBase
    {
        private static readonly ActionDispatcherBase _singleton = new ActionDispatcher();

        public new static ActionResult Dispatch(Controller sourceController, object viewModel, [CallerMemberName] string sourceActionName = "")
            => _singleton.Dispatch(sourceController, viewModel, sourceActionName);

        protected override IList<(Type viewModelType, string controllerName, string httpGetActionName, string viewName)> DispatchTuples { get; }
            = new[]
            {
                (typeof(CategorySelectorViewModel), nameof(ControllerNames.CategorySelector), nameof(ActionNames.Index), nameof(ViewNames.Index)),
                (typeof(LoginViewModel), nameof(ControllerNames.Login), nameof(ActionNames.Index), nameof(ViewNames.Index)),
                (typeof(QuestionEditViewModel), nameof(ControllerNames.Questions), nameof(ActionNames.Edit), nameof(ViewNames.Edit)),
                (typeof(QuestionConfirmDeleteViewModel), nameof(ControllerNames.Questions), nameof(ActionNames.Delete), nameof(ViewNames.Delete)),
                (typeof(QuestionDeleteConfirmedViewModel), nameof(ControllerNames.Questions), null, nameof(ViewNames.Edit)),
                (typeof(QuestionDetailsViewModel), nameof(ControllerNames.Questions), nameof(ActionNames.Details), nameof(ViewNames.Details)),
                (typeof(QuestionListViewModel), nameof(ControllerNames.Questions), nameof(ActionNames.Index), nameof(ViewNames.Index)),
                (typeof(QuestionNotFoundViewModel), nameof(ControllerNames.Questions), null, nameof(ViewNames.NotFound)),
                (typeof(RandomQuestionViewModel), nameof(ControllerNames.Questions), null, nameof(ViewNames.Random))
            };

        protected override object TryGetRouteValues(object viewModel)
        {
            switch (viewModel)
            {
                case QuestionEditViewModel castedViewModel: return new { id = castedViewModel.Question.ID, ret = castedViewModel.ReturnAction };
                case QuestionConfirmDeleteViewModel castedViewModel: return new { id = castedViewModel.ID };
                case QuestionDetailsViewModel castedViewModel: return new { id = castedViewModel.Question.ID };
                case QuestionListViewModel castedViewModel: return new { page = castedViewModel.Pager.PageNumber };
                case RandomQuestionViewModel castedViewModel: return new { c = castedViewModel.SelectedCategories.Select(x => x.ID).ToArray() };
            }

            return null;
        }

        protected override IList<string> GetValidationMesssages(object viewModel)
        {
            if (viewModel is QuestionEditViewModel castedViewModel && castedViewModel.ValidationMessages.Any())
            {
                return new[]
                {
                    "An error occurred. This is a placeholder to let MVC know something is wrong. " +
                    "The validation messages are custom-managed by application code, not passed on to the MVC framework."
                };
            }

            return Array.Empty<string>();
        }
    }
}