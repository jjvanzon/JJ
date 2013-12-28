using JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers
{
    public class SmallLoginSubController
    {
        private HttpSessionStateBase _session;

        public SmallLoginSubController(HttpSessionStateBase session)
        {
            if (session == null) { throw new ArgumentNullException("session"); }

            _session = session;
        }

        public SmallLoginViewModel GetSmallLoginViewModel()
        {
            return GetSessionWrapper().SmallLoginViewModel;
        }

        // TODO: Replace ensure with just an auto-instantiating getter?
        public void EnsureSmallLoginViewModel()
        {
            SmallLoginViewModel viewModel = GetSmallLoginViewModel();

            if (viewModel == null)
            {
                SmallLoginPresenter presenter = CreateSmallLoginPresenter();
                viewModel = presenter.Show();
                GetSessionWrapper().SmallLoginViewModel = viewModel;
            }
        }

        public void SetLoggedInUserName(string authenticatedUserName)
        {
            SmallLoginPresenter presenter = CreateSmallLoginPresenter();
            SmallLoginViewModel viewModel = presenter.SetLoggedInUserName(authenticatedUserName);
            GetSessionWrapper().SmallLoginViewModel = viewModel;
        }

        public void SetIsLoggedOut()
        {
            SmallLoginPresenter presenter = CreateSmallLoginPresenter();
            SmallLoginViewModel viewModel = presenter.SetIsLoggedOut();
            GetSessionWrapper().SmallLoginViewModel = viewModel;
        }

        // Helpers

        private SmallLoginPresenter CreateSmallLoginPresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            SmallLoginPresenter presenter = new SmallLoginPresenter(userRepository);
            return presenter;
        }

        private SessionWrapper GetSessionWrapper()
        {
            return new SessionWrapper(_session);
        }
    }
}