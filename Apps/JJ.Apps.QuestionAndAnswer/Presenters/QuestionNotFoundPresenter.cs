using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionNotFoundPresenter
    {
        private string _authenticatedUserName;
        private IUserRepository _userRepository;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionNotFoundPresenter(string authenticatedUserName, IUserRepository userRepository)
        {
            if (userRepository == null) throw new NullException(() => userRepository);

            _userRepository = userRepository;
            _authenticatedUserName = authenticatedUserName;
        }

        public QuestionNotFoundViewModel Show()
        {
            var viewModel = new QuestionNotFoundViewModel();
            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _userRepository);
            viewModel.LanguageSelector = ViewModelHelper.CreateLanguageSelectionViewModel();
            return viewModel;
        }

        public QuestionNotFoundViewModel SetLanguage(string cultureName)
        {
            CultureHelper.SetCulture(cultureName);
            return Show();
        }
    }
}
