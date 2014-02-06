using JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class SmallLoginSubController
    {
        private readonly Controller _parentController;

        public SmallLoginSubController(Controller parentController)
        {
            if (parentController == null) { throw new ArgumentNullException("parentController"); }

            _parentController = parentController;
        }

        public SmallLoginViewModel Model
        {
            get
            {
                SmallLoginViewModel viewModel = GetSessionWrapper().SmallLoginViewModel;
                if (viewModel != null)
                {
                    return viewModel;
                }

                using (UserRepositoryWrapper userRepositoryWrapper = CreateRepositoryWrapper())
                {
                    var presenter = new SmallLoginPresenter(userRepositoryWrapper.UserRepository);
                    viewModel = presenter.Show();
                    GetSessionWrapper().SmallLoginViewModel = viewModel;
                    return viewModel;
                }
            }
            private set
            {
                GetSessionWrapper().SmallLoginViewModel = value;
            }
        }

        public void SetLoggedInUserName(string authenticatedUserName)
        {
            using (UserRepositoryWrapper repositoryWrapper = CreateRepositoryWrapper())
            {
                using (UserRepositoryWrapper userRepositoryWrapper = CreateRepositoryWrapper())
                {
                    SmallLoginPresenter presenter = new SmallLoginPresenter(userRepositoryWrapper.UserRepository);
                    SmallLoginViewModel viewModel = presenter.SetLoggedInUserName(authenticatedUserName);
                    Model = viewModel;
                }
            }
        }

        public void SetIsLoggedOut()
        {
            using (UserRepositoryWrapper repositoryWrapper = CreateRepositoryWrapper())
            {
                SmallLoginPresenter presenter = new SmallLoginPresenter(repositoryWrapper.UserRepository);
                SmallLoginViewModel viewModel = presenter.SetIsLoggedOut();
                Model = viewModel;
            }
        }

        // Helpers

        private UserRepositoryWrapper CreateRepositoryWrapper()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            IUserRepository userRepository = new UserRepository(context);
            return new UserRepositoryWrapper(userRepository, context);
        }

        private SessionWrapper GetSessionWrapper()
        {
            return new SessionWrapper(_parentController.Session);
        }
    }
}