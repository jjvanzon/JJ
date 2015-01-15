using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters.Partials
{
    public class SmallLoginPartialPresenter
    {
        private IUserRepository _userRepository;

        public SmallLoginPartialPresenter(IUserRepository userRepository)
        {
            if (userRepository == null) throw new NullException(() => userRepository);

            _userRepository = userRepository;
        }

        public LoginPartialViewModel Show()
        {
            return CreateViewModel();
        }

        public LoginPartialViewModel ShowLoggedOut()
        {
            return CreateViewModel();
        }

        public LoginPartialViewModel ShowLoggedIn(string userName)
        {
            User user = _userRepository.GetByUserName(userName);

            var viewModel = new LoginPartialViewModel 
            { 
                CanLogOut = true,
                UserDisplayName = user.DisplayName 
            };

            return viewModel;
        }

        private LoginPartialViewModel CreateViewModel()
        {
            return new LoginPartialViewModel { CanLogIn = true };
        }
    }
}
