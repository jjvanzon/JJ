using JJ.Apps.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class NotAuthenticatedPresenter
    {
        public NotAuthenticatedViewModel Show()
        {
            return new NotAuthenticatedViewModel();
        }
    }
}
