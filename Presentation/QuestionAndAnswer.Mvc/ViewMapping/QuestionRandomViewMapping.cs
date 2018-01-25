using JJ.Framework.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
	public class QuestionRandomViewMapping : ViewMapping<RandomQuestionViewModel>
	{
		public QuestionRandomViewMapping()
		{
			MapPresenter(PresenterNames.RandomQuestionPresenter, PresenterActionNames.Edit);
			MapController(ControllerNames.Questions, ActionNames.Random, ViewNames.Random);
			MapParameter(PresenterParameterNames.categoryIDs, ActionParameterNames.c);
		}
	}
}