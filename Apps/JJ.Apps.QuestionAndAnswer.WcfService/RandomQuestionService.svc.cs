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

namespace JJ.Apps.QuestionAndAnswer.WcfService
{
    public class RandomQuestionService : IRandomQuestionService
    {
        public RandomQuestionViewModel ShowQuestion()
        {
            RandomQuestionPresenter presenter = CreatePresenter();
            return presenter.Show();
        }

        public RandomQuestionViewModel ShowAnswer(RandomQuestionViewModel viewModel)
        {
            RandomQuestionPresenter presenter = CreatePresenter();
            return presenter.ShowAnswer(viewModel, null);
        }

        public RandomQuestionViewModel HideAnswer(RandomQuestionViewModel viewModel)
        {
            RandomQuestionPresenter presenter = CreatePresenter();
            return presenter.HideAnswer(viewModel, null);
        }

        private RandomQuestionPresenter CreatePresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            ICategoryRepository categoryRepository = RepositoryFactory.CreateCategoryRepository(context);
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            IQuestionFlagRepository questionFlagRepository = RepositoryFactory.CreateQuestionFlagRepository(context);
            IFlagStatusRepository flagStatusRepository = RepositoryFactory.CreateFlagStatusRepository(context);
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            return new RandomQuestionPresenter(questionRepository, categoryRepository, questionFlagRepository, flagStatusRepository, userRepository);
        }
    }
}
