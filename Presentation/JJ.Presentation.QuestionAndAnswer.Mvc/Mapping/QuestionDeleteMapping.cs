using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Mapping
{
    public class QuestionDeleteMapping : ViewMapping<QuestionConfirmDeleteViewModel>
    {
        public QuestionDeleteMapping()
            : base(ViewNames.Delete)
        {
            PresenterName = PresenterNames.QuestionConfirmDeletePresenter;
            PresenterActionName = PresenterActionNames.Show;
            ControllerName = ControllerNames.Questions;
            ControllerGetActionName = ActionNames.Delete;
        }

        protected override object GetRouteValues(QuestionConfirmDeleteViewModel viewModel)
        {
            return new { id = viewModel.ID };
        }
    }
}