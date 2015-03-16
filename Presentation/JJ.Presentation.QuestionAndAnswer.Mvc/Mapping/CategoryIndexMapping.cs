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
    public class CategoryIndexMapping : ViewMapping<CategorySelectorViewModel>
    {
        public CategoryIndexMapping()
            : base(ViewNames.Index)
        {
            PresenterName = PresenterNames.CategorySelectorPresenter;
            PresenterActionName = PresenterActionNames.Show;
            ControllerName = ControllerNames.CategorySelector;
            ControllerGetActionName = ActionNames.Index;
        }
    }
}