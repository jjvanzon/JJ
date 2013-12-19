using JJ.Apps.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class SmallLoginPresenter
    {
        public LoginViewModel Show()
        {
            return new LoginViewModel { IsLoggedIn = false };
        }
    }
}
