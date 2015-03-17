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
    public class QuestionRandomMapping : ViewMapping<RandomQuestionViewModel>
    {
        public QuestionRandomMapping()
            : base(ViewNames.Random)
        {
            PresenterName = PresenterNames.RandomQuestionPresenter;
            PresenterActionName = PresenterActionNames.Edit;

            ControllerName = ControllerNames.Questions;
            ControllerGetActionName = ActionNames.Random;
        }
    }
}