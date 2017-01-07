using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class QuestionNotFoundViewMapping : ViewMapping<QuestionNotFoundViewModel>
    {
        public QuestionNotFoundViewMapping()
        {
            MapController(ControllerNames.Questions, ViewNames.NotFound);
        }
    }
}