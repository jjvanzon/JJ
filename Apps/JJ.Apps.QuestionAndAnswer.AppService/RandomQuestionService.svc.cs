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
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;

namespace JJ.Apps.QuestionAndAnswer.AppService
{
    public class RandomQuestionService : IRandomQuestionService
    {
        public RandomQuestionViewModel ShowQuestion()
        {
            using (IContext context = ContextHelper.CreateContextFromConfiguration())
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
            using (IContext context = ContextHelper.CreateContextFromConfiguration())
            {
                RandomQuestionPresenter presenter = CreatePresenter(context);
                // TODO: Polymorphic results.
                object viewModel2 = presenter.ShowAnswer(viewModel, null);
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
            using (IContext context = ContextHelper.CreateContextFromConfiguration())
            {
                RandomQuestionPresenter presenter = CreatePresenter(context);

                // TODO: Polymorphic results.
                object viewModel2 = presenter.HideAnswer(viewModel, null);
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
                new QuestionRepository(context, context.Location),
                new CategoryRepository(context),
                new QuestionFlagRepository(context),
                new FlagStatusRepository(context),
                new UserRepository(context));
        }
    }
}
