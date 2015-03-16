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
    public class QuestionDetailsMapping : ViewMapping<QuestionDetailsViewModel>
    {
        public QuestionDetailsMapping()
            : base(ViewNames.Details)
        {
            PresenterName = PresenterNames.QuestionDetailsPresenter;
            PresenterActionName = PresenterActionNames.Show;
            ControllerName = ControllerNames.Questions;
            ControllerGetActionName = ActionNames.Details;
        }

        protected override object GetRouteValues(QuestionDetailsViewModel viewModel)
        {
            return new { id = viewModel.Question.ID };
        }
    }
}