﻿using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.ViewMapping
{
    public class QuestionRandomViewMapping : ViewMapping<RandomQuestionViewModel>
    {
        public QuestionRandomViewMapping()
        {
            MapPresenter(PresenterNames.RandomQuestionPresenter, PresenterActionNames.Edit);
            MapController(ControllerNames.Questions, ActionNames.Random, ViewNames.Random);
        }
    }
}