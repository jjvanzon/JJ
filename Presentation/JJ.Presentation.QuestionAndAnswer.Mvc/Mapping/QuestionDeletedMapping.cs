using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Mapping
{
    public class QuestionDeletedMapping : ViewMapping<QuestionDeleteConfirmedViewModel>
    {
        public QuestionDeletedMapping()
            : base(ViewNames.Deleted)
        {
            ControllerName = ControllerNames.Questions;
        }
    }
}