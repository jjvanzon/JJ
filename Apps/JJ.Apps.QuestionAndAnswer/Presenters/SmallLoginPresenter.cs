using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
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
        private IUserRepository _userRepository;

        public SmallLoginPresenter(IUserRepository userRepository)
        {
            if (userRepository == null) { throw new ArgumentNullException("userRepository"); }

            _userRepository = userRepository;
        }

        public SmallLoginViewModel Show()
        {
            return CreateViewModel();
        }

        public SmallLoginViewModel SetLoggedInUserName(string userName)
        {
            User user = _userRepository.GetByUserName(userName);

            var viewModel = new SmallLoginViewModel 
            { 
                LogOutActionIsVisible = true,
                DisplayName = user.DisplayName 
            };

            return viewModel;
        }

        public SmallLoginViewModel SetIsLoggedOut()
        {
            return CreateViewModel();   
        }

        private SmallLoginViewModel CreateViewModel()
        {
            return new SmallLoginViewModel { LogInActionIsVisible = true };
        }
    }
}
