using JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
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

                if (viewModel == null)
                {
                    SmallLoginPresenter presenter = CreateSmallLoginPresenter();
                    viewModel = presenter.Show();
                    GetSessionWrapper().SmallLoginViewModel = viewModel;
                }

                return viewModel;
            }
            private set
            {
                GetSessionWrapper().SmallLoginViewModel = value;
            }
        }

        public void SetLoggedInUserName(string authenticatedUserName)
        {
            SmallLoginPresenter presenter = CreateSmallLoginPresenter();
            SmallLoginViewModel viewModel = presenter.SetLoggedInUserName(authenticatedUserName);
            Model = viewModel;
        }

        public void SetIsLoggedOut()
        {
            SmallLoginPresenter presenter = CreateSmallLoginPresenter();
            SmallLoginViewModel viewModel = presenter.SetIsLoggedOut();
            Model = viewModel;
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
            return new SessionWrapper(_parentController.Session);
        }
    }
}