using JJ.Framework.Presentation;
using JJ.Framework.Presentation.Mvc;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Mapping
{
    public class NotAuthorizedMapping : ViewMapping<NotAuthorizedViewModel>
    {
        public NotAuthorizedMapping()
            : base(ViewNames.NotAuthorized)
        { }
    }
}