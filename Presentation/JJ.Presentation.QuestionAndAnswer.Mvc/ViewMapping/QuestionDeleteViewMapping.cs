using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class QuestionDeleteViewMapping : ViewMapping<QuestionConfirmDeleteViewModel>
    {
        public QuestionDeleteViewMapping()
        {
            MapPresenter(PresenterNames.QuestionConfirmDeletePresenter, PresenterActionNames.Show);
            MapController(ControllerNames.Questions, ActionNames.Delete, ViewNames.Delete);
        }

        protected override object GetRouteValues(QuestionConfirmDeleteViewModel viewModel)
        {
            return new { id = viewModel.ID };
        }
    }
}