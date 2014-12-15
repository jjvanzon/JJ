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

namespace JJ.Apps.QuestionAndAnswer.Mvc.Controllers
{
    public class SmallLoginSubController
    {
        private readonly Controller _parentController;

        public SmallLoginSubController(Controller parentController)
        {
            if (parentController == null) throw new ArgumentNullException("parentController");

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

                using (IContext context = PersistenceHelper.CreateContext())
                {
                    IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                    SmallLoginPresenter presenter = new SmallLoginPresenter(userRepository);
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
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                SmallLoginPresenter presenter = new SmallLoginPresenter(userRepository);
                SmallLoginViewModel viewModel = presenter.SetLoggedInUserName(authenticatedUserName);
                Model = viewModel;
            }
        }

        public void SetIsLoggedOut()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IUserRepository userRepository = PersistenceHelper.CreateRepository<IUserRepository>(context);
                SmallLoginPresenter presenter = new SmallLoginPresenter(userRepository);
                SmallLoginViewModel viewModel = presenter.SetIsLoggedOut();
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