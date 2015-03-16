using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Mapping
{
    public class QuestionEditMapping : ViewMapping<QuestionEditViewModel>
    {
        public QuestionEditMapping()
            : base(ViewNames.Edit)
        {
            PresenterName = PresenterNames.QuestionDetailsPresenter;
            PresenterActionName = PresenterActionNames.Edit;
            ControllerName = ControllerNames.Questions;
            ControllerGetActionName = ActionNames.Edit;
        }

        protected override bool Predicate(QuestionEditViewModel viewModel)
        {
            return !viewModel.IsNew;
        }

        protected override object GetRouteValues(QuestionEditViewModel viewModel)
        {
            return new 
            { 
                id = viewModel.Question.ID, 
                ret = GetReturnUrl(viewModel.ReturnAction) 
            };
        }

        protected override ICollection<KeyValuePair<string, string>> GetValidationMesssages(QuestionEditViewModel viewModel)
        {
            return viewModel.ValidationMessages.ToKeyValuePairs();
        }
    }
}