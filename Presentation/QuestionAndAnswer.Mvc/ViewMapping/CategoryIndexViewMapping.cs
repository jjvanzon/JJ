using JJ.Framework.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
	public class CategoryIndexViewMapping : ViewMapping<CategorySelectorViewModel>
	{
		public CategoryIndexViewMapping()
		{
			MapPresenter(PresenterNames.CategorySelectorPresenter, PresenterActionNames.Show);
			MapController(ControllerNames.CategorySelector, ActionNames.Index, ViewNames.Index);
		}
	}
}