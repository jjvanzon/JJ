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
    public class QuestionIndexMapping : ViewMapping<QuestionListViewModel>
    {
        public QuestionIndexMapping()
            : base(ViewNames.Index)
        {
            PresenterName = PresenterNames.QuestionListPresenter;
            PresenterActionName = PresenterActionNames.Show;
            ControllerName = ControllerNames.Questions;
            ControllerGetActionName = ActionNames.Index;
        }

        protected override object GetRouteValues(QuestionListViewModel viewModel)
        {
            return new { page = viewModel.Pager.PageNumber };
        }
    }
}