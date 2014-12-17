using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Models.QuestionAndAnswer.Repositories;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;

namespace JJ.Apps.QuestionAndAnswer.AppService
{
    public class RandomQuestionService : IRandomQuestionService
    {
        public RandomQuestionViewModel ShowQuestion()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                RandomQuestionPresenter presenter = CreatePresenter(context);

                // TODO: Polymorphic results.
                object viewModel = presenter.Show();
                if (viewModel is RandomQuestionViewModel)
                {
                    return viewModel as RandomQuestionViewModel;
                }
                else
                {
                    return new RandomQuestionViewModel { Question = new QuestionViewModel() };
                }
            }
        }

        public RandomQuestionViewModel ShowAnswer(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                RandomQuestionPresenter presenter = CreatePresenter(context);

                // TODO: Polymorphic results.
                object viewModel2 = presenter.ShowAnswer(viewModel);
                if (viewModel2 is RandomQuestionViewModel)
                {
                    return viewModel2 as RandomQuestionViewModel;
                }
                else
                {
                    return new RandomQuestionViewModel { Question = new QuestionViewModel() };
                }
            }
        }

        public RandomQuestionViewModel HideAnswer(RandomQuestionViewModel viewModel)
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                RandomQuestionPresenter presenter = CreatePresenter(context);

                // TODO: Polymorphic results.
                object viewModel2 = presenter.HideAnswer(viewModel);
                if (viewModel2 is RandomQuestionViewModel)
                {
                    return viewModel2 as RandomQuestionViewModel;
                }
                else
                {
                    return new RandomQuestionViewModel { Question = new QuestionViewModel() };
                }
            }
        }

        private RandomQuestionPresenter CreatePresenter(IContext context)
        {
            return new RandomQuestionPresenter(
                PersistenceHelper.CreateRepository<IQuestionRepository>(context),
                PersistenceHelper.CreateRepository<ICategoryRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionFlagRepository>(context),
                PersistenceHelper.CreateRepository<IFlagStatusRepository>(context),
                PersistenceHelper.CreateRepository<IUserRepository>(context),
                authenticatedUserName: null);
        }
    }
}
