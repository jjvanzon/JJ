using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Apps.QuestionAndAnswer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Framework.Reflection;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionListPresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;
        private int _pageSize;
        private int _maxVisiblePageNumbers;

        public QuestionListPresenter(
            Repositories repositories, 
            string authenticatedUserName, 
            int pageSize,
            int maxVisiblePageNumbers)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _authenticatedUserName = authenticatedUserName;
            _pageSize = pageSize;
            _maxVisiblePageNumbers = maxVisiblePageNumbers;
        }

        public QuestionListViewModel Show(int pageNumber = 1)
        {
            var viewModel = new QuestionListViewModel();
            viewModel.List = new List<QuestionViewModel>();

            int pageIndex = pageNumber - 1;

            foreach (Question question in _repositories.QuestionRepository.GetPage(pageIndex * _pageSize, _pageSize))
            {
                QuestionViewModel itemViewModel = question.ToViewModel();
                itemViewModel.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();
                viewModel.List.Add(itemViewModel);
            }

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _repositories.UserRepository);

            int count = _repositories.QuestionRepository.CountAll();
            viewModel.Paging = ViewModelHelper.CreatePagingViewModel(pageIndex, _pageSize, count, _maxVisiblePageNumbers);

            return viewModel;
        }

        public QuestionListViewModel Filter(bool? isFlagged)
        {
            // TODO: We probably need more criteria.
            bool mustFilterByFlagStatusID = isFlagged.HasValue;
            int? flagStatusID = null;
            if (isFlagged.HasValue)
            {
                if (isFlagged.Value == true)
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
            var detailPresenter = new QuestionDetailsPresenter(_repositories, _authenticatedUserName, _pageSize, _maxVisiblePageNumbers);
            return detailPresenter.Show(questionID);
        }

        public object Edit(int questionID)
        {
            var editPresenter = new QuestionEditPresenter(_repositories, _authenticatedUserName, _pageSize, _maxVisiblePageNumbers);
            return editPresenter.Edit(questionID);
        }

        public object Delete(int questionID)
        {
            var deletePresenter = new QuestionConfirmDeletePresenter(_repositories, _authenticatedUserName, _pageSize, _maxVisiblePageNumbers);
            return deletePresenter.Show(questionID);
        }

        public object SetLanguage(string cultureName)
        {
            CultureHelper.SetCulture(cultureName);
            // TODO: Filter parameters should be yielded over once they are used.
            return Show();
        }
    }
}
