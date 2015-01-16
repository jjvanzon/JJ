using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ToViewModel
{
    internal static class UserExtensions
    {
        public static LoginPartialViewModel ToLoginPartialViewModel(this User user)
        {
            if (user == null) throw new NullException(() => user);

            return new LoginPartialViewModel 
            {
                UserDisplayName = user.DisplayName, 
                CanLogOut = true 
            };
        }
    }
}
