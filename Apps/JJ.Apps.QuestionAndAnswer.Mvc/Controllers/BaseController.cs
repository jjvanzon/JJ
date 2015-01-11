using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection;
using JJ.Models.Canonical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        // View()

        private Dictionary<Type, string> _viewNameDictionary = new Dictionary<Type, string>()
        {
            { typeof(CategorySelectorViewModel), ViewNames.Index },
            { typeof(LoginViewModel), ViewNames.Index },
            { typeof(QuestionConfirmDeleteViewModel), ViewNames.Delete },
            { typeof(QuestionDeleteConfirmedViewModel), ViewNames.Deleted },
            { typeof(QuestionDetailsViewModel), ViewNames.Details },
            { typeof(QuestionListViewModel), ViewNames.Index },
            { typeof(QuestionNotFoundViewModel), ViewNames.NotFound },
            { typeof(RandomQuestionViewModel), ViewNames.Random }
        };

        public ViewResult ViewPolymorphic(object viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            string viewName;
            if (_viewNameDictionary.TryGetValue(viewModel.GetType(), out viewName))
            {
                return View(viewName, viewModel);
            }

            var questionEditViewModel = viewModel as QuestionEditViewModel;
            if (questionEditViewModel != null)
            {
                foreach (ValidationMessage validationMessage in questionEditViewModel.ValidationMessages)
                {
                    ModelState.AddModelError(validationMessage.PropertyKey, validationMessage.Text);
                }

                return View(ViewNames.Edit, viewModel);
            }

            throw new UnexpectedViewModelTypeException(viewModel);
        }

        private Dictionary<Type, string> _actionNameDictionary = new Dictionary<Type, string>()
        {
            { typeof(CategorySelectorViewModel), ActionNames.Index },
            { typeof(LoginViewModel), ActionNames.Index },
            { typeof(QuestionConfirmDeleteViewModel), ActionNames.Delete },
            //{ typeof(QuestionDeleteConfirmedViewModel), ActionNames.Deleted },
            { typeof(QuestionListViewModel), ActionNames.Index },
            //{ typeof(QuestionNotFoundViewModel), ActionNames.NotFound },
            { typeof(RandomQuestionViewModel), ActionNames.Random }
        };

        // RedirectToAction

        public ActionResult RedirectToActionPolymorphic(object viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TempData[TempDataKeys.ViewModel] = viewModel;

            string actionName;
            if (_actionNameDictionary.TryGetValue(viewModel.GetType(), out actionName))
            {
                return RedirectToAction(actionName);
            }

            var questionEditViewModel = viewModel as QuestionEditViewModel;
            if (questionEditViewModel != null)
            {
                if (questionEditViewModel.IsNew)
                {
                    return RedirectToAction(ActionNames.Create);
                }
                else
                {
                    return RedirectToAction(ActionNames.Edit, new { id = questionEditViewModel.Question.ID });
                }
            }

            var questionDetailsViewModel = viewModel as QuestionDetailsViewModel;
            if (questionDetailsViewModel != null)
            {
                return RedirectToAction(ActionNames.Details, new { id = questionDetailsViewModel.Question.ID });
            }

            throw new UnexpectedViewModelTypeException(viewModel);
        }
    }
}