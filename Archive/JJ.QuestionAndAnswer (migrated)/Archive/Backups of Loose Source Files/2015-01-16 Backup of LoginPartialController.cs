using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JJ.Framework.Persistence;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer.Repositories;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Apps.QuestionAndAnswer.Mvc.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.Presenters.Partials;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using JJ.Apps.QuestionAndAnswer.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class LoginPartialController
    {
        private readonly Controller _parentController;

        public LoginPartialController(Controller parentController)
        {
            if (parentController == null) throw new NullException(() => parentController);

            _parentController = parentController;
        }

        public LoginPartialViewModel Model
        {
            get
            {
                LoginPartialViewModel viewModel = GetSessionWrapper().LoginPartialViewModel;
                if (viewModel == null)
                {
                    using (IContext context = PersistenceHelper.CreateContext())
                    {
                        IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                        LoginPartialPresenter presenter = new LoginPartialPresenter(userRepository);
                        viewModel = presenter.ShowLoggedOut();
                        GetSessionWrapper().LoginPartialViewModel = viewModel;
                        return viewModel;
                    }
                }

                return viewModel;
            }
            private set
            {
                GetSessionWrapper().LoginPartialViewModel = value;
            }
        }

        public void ShowLoggedIn(string authenticatedUserName)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                LoginPartialPresenter presenter = new LoginPartialPresenter(userRepository);
                LoginPartialViewModel viewModel = presenter.ShowLoggedIn(authenticatedUserName);
                Model = viewModel;
            }
        }

        public void ShowLoggedOut()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                LoginPartialPresenter presenter = new LoginPartialPresenter(userRepository);
                LoginPartialViewModel viewModel = presenter.ShowLoggedOut();
                Model = viewModel;
            }
        }

        // Helpers

        private SessionWrapper GetSessionWrapper()
        {
            return new SessionWrapper(_parentController.Session);
        }
    }
}