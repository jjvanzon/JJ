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
    public class QuesionService : IQuestionService
    {
        public QuestionDetailViewModel ShowQuestion()
        {
            QuestionPresenter presenter = CreatePresenter();
            return presenter.ShowQuestion();
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            QuestionPresenter presenter = CreatePresenter();
            return presenter.ShowAnswer(viewModel);
        }

        public QuestionDetailViewModel HideAnswer(QuestionDetailViewModel viewModel)
        {
            QuestionPresenter presenter = CreatePresenter();
            return presenter.HideAnswer(viewModel);
        }

        private QuestionPresenter CreatePresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            ICategoryRepository categoryRepository = RepositoryFactory.CreateCategoryRepository(context);
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            IQuestionFlagRepository questionFlagRepository = RepositoryFactory.CreateQuestionFlagRepository(context);
            IFlagStatusRepository flagStatusRepository = RepositoryFactory.CreateFlagStatusRepository(context);
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            return new QuestionPresenter(questionRepository, categoryRepository, questionFlagRepository, flagStatusRepository, userRepository);
        }
    }
}
