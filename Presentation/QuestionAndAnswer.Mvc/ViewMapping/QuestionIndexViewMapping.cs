using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class QuestionIndexViewMapping : ViewMapping<QuestionListViewModel>
    {
        public QuestionIndexViewMapping()
        {
            MapPresenter(PresenterNames.QuestionListPresenter, PresenterActionNames.Show);
            MapController(ControllerNames.Questions, ActionNames.Index, ViewNames.Index);
            MapParameter(PresenterParameterNames.pageNumber, ActionParameterNames.page);
        }

        protected override object GetRouteValues(QuestionListViewModel viewModel)
        {
            return new { page = viewModel.Pager.PageNumber };
        }
    }
}