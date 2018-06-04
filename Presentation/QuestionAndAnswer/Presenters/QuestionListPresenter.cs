using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Configuration;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Presentation;
using JJ.Presentation.QuestionAndAnswer.Configuration;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionListPresenter
    {
        private static readonly int _pageSize = CustomConfigurationManager.GetSection<ConfigurationSection>().PageSize;
        private static readonly int _maxVisiblePageNumbers = CustomConfigurationManager.GetSection<ConfigurationSection>().MaxVisiblePageNumbers;

        private readonly Repositories _repositories;
        private readonly string _authenticatedUserName;

        public QuestionListPresenter(Repositories repositories, string authenticatedUserName)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
            _authenticatedUserName = authenticatedUserName;
        }

        public QuestionListViewModel Show(int pageNumber = 1)
        {
            var viewModel = new QuestionListViewModel
            {
                List = new List<QuestionViewModel>()
            };

            int pageIndex = pageNumber - 1;

            IList<Question> questions = _repositories.QuestionRepository.GetPage(pageIndex * _pageSize, _pageSize);

            foreach (Question question in questions)
            {
                QuestionViewModel itemViewModel = question.ToViewModel();
                itemViewModel.IsFlagged = question.QuestionFlags.Where(x => x.GetFlagStatusEnum() == FlagStatusEnum.Flagged).Any();
                viewModel.List.Add(itemViewModel);
            }

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _repositories.UserRepository);

            int count = _repositories.QuestionRepository.Count();
            viewModel.Pager = PagerViewModelFactory.Create(pageIndex, _pageSize, count, _maxVisiblePageNumbers);

            return viewModel;
        }

        public QuestionListViewModel Filter(bool? isFlagged)
        {
            // TODO: We probably need more criteria.
            bool mustFilterByFlagStatusID = isFlagged.HasValue;
            int? flagStatusID = null;
            if (isFlagged.HasValue)
            {
                if (isFlagged.Value)
                {
                    flagStatusID = (int)FlagStatusEnum.Flagged;
                }
            }

            var viewModel = new QuestionListViewModel();
            IList<Question> questions = _repositories.QuestionRepository.GetByCriteria(mustFilterByFlagStatusID, flagStatusID);
            viewModel.List = questions.Select(x => x.ToViewModel()).ToArray();
            return viewModel;
        }

        public object Details(int questionID)
        {
            var detailPresenter = new QuestionDetailsPresenter(_repositories, _authenticatedUserName);
            return detailPresenter.Show(questionID);
        }

        public object Create()
        {
            var presenter2 = new QuestionEditPresenter(_repositories, _authenticatedUserName);
            return presenter2.Create();
        }

        public object Edit(int questionID, int pageNumber)
        {
            var editPresenter = new QuestionEditPresenter(_repositories, _authenticatedUserName);
            return editPresenter.Edit(questionID, CreateReturnAction(() => Show(pageNumber)));
        }

        public object Delete(int questionID)
        {
            var deletePresenter = new QuestionConfirmDeletePresenter(_repositories, _authenticatedUserName);
            return deletePresenter.Show(questionID);
        }

        private ActionInfo CreateReturnAction(Expression<Func<object>> methodCallExpression)
            => ActionDispatcher.CreateActionInfo(GetType(), methodCallExpression);
    }
}