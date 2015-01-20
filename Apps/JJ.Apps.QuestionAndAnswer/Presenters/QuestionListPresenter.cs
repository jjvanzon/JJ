using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
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

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionListPresenter
    {
        private Repositories _repositories;
        private string _authenticatedUserName;

        public QuestionListPresenter(Repositories repositories, string authenticatedUserName)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _authenticatedUserName = authenticatedUserName;
        }

        public QuestionListViewModel Show()
        {
            var viewModel = new QuestionListViewModel();
            viewModel.List = new List<QuestionViewModel>();

            foreach (Question question in _repositories.QuestionRepository.GetAll())
            {
                QuestionViewModel itemViewModel = question.ToViewModel();
                itemViewModel.IsFlagged = question.QuestionFlags.Where(x => x.FlagStatus.ID == (int)FlagStatusEnum.Flagged).Any();
                viewModel.List.Add(itemViewModel);
            }

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _repositories.UserRepository);
            viewModel.LanguageSelector = ViewModelHelper.CreateLanguageSelectionViewModel();

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
            IEnumerable<Question> questions = _repositories.QuestionRepository.GetByCriteria(mustFilterByFlagStatusID, flagStatusID);
            viewModel.List = questions.Select(x => x.ToViewModel()).ToArray();
            return viewModel;
        }

        public object Details(int questionID)
        {
            var detailPresenter = new QuestionDetailsPresenter(_repositories, _authenticatedUserName);
            return detailPresenter.Show(questionID);
        }

        public object Edit(int questionID)
        {
            var editPresenter = new QuestionEditPresenter(_repositories, _authenticatedUserName);
            return editPresenter.Edit(questionID);
        }

        public object Delete(int questionID)
        {
            var deletePresenter = new QuestionConfirmDeletePresenter(_repositories, _authenticatedUserName);
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
