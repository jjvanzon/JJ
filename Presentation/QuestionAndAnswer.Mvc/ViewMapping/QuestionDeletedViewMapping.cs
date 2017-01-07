using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class QuestionDeletedViewMapping : ViewMapping<QuestionDeleteConfirmedViewModel>
    {
        public QuestionDeletedViewMapping()
        {
            MapController(ControllerNames.Questions, ViewNames.Deleted);
        }
    }
}