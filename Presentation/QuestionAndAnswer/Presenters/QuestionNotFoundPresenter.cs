using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionNotFoundPresenter
    {
        private string _authenticatedUserName;
        private IUserRepository _userRepository;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionNotFoundPresenter(IUserRepository userRepository, string authenticatedUserName)
        {
            if (userRepository == null) throw new NullException(() => userRepository);

            _userRepository = userRepository;
            _authenticatedUserName = authenticatedUserName;
        }

        public QuestionNotFoundViewModel Show()
        {
            var viewModel = new QuestionNotFoundViewModel();
            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _userRepository);
            return viewModel;
        }

        public QuestionNotFoundViewModel SetLanguage(string cultureName)
        {
            CultureHelper.SetCulture(cultureName);
            return Show();
        }
    }
}
