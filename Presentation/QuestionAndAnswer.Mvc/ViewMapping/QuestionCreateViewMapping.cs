using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System.Collections.Generic;
using JJ.Business.Canonical;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class QuestionCreateViewMapping : ViewMapping<QuestionEditViewModel>
    {
        public QuestionCreateViewMapping()
            : base()
        { 
            MapPresenter(PresenterNames.QuestionEditPresenter, PresenterActionNames.Create);
            MapController(ControllerNames.Questions, ActionNames.Create, ViewNames.Edit);
        }

        protected override bool Predicate(QuestionEditViewModel viewModel)
        {
            return viewModel.IsNew;
        }

        protected override object GetRouteValues(QuestionEditViewModel viewModel)
        {
            return new 
            { 
                id = viewModel.Question.ID, 
                ret = TryGetReturnUrl(viewModel.ReturnAction) 
            };
        }

        protected override ICollection<string> GetValidationMesssages(QuestionEditViewModel viewModel)
        {
            return viewModel.ValidationMessages;
        }
    }
}
