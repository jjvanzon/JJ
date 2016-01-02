using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JJ.Business.Canonical;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class QuestionEditViewMapping : ViewMapping<QuestionEditViewModel>
    {
        public QuestionEditViewMapping()
        {
            MapPresenter(PresenterNames.QuestionEditPresenter, PresenterActionNames.Edit);
            MapController(ControllerNames.Questions, ActionNames.Edit, ViewNames.Edit);
            MapParameter(PresenterParameterNames.id, ActionParameterNames.id);
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
                ret = TryGetReturnUrl(viewModel.ReturnAction) 
            };
        }

        protected override ICollection<KeyValuePair<string, string>> GetValidationMesssages(QuestionEditViewModel viewModel)
        {
            return viewModel.ValidationMessages.ToKeyValuePairs();
        }
    }
}