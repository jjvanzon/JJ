using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionDeleteConfirmedPresenter
    {
        private Repositories _repositories;

        public QuestionDeleteConfirmedPresenter(Repositories repositories)
        {
            if (repositories == null) throw new ArgumentNullException("repositories");

            _repositories = repositories;
        }
        
        /// <summary> Can return QuestionDeleteConfirmedViewModel and QuestionNotFoundViewModel. </summary>
        public object Show(int id)
        {
            Question question = _repositories.QuestionRepository.TryGet(id);
            if (question == null)
            {
                return new QuestionNotFoundViewModel { ID = id };
            }

            question.DeleteRelatedEntities(_repositories.AnswerRepository, _repositories.QuestionCategoryRepository, _repositories.QuestionLinkRepository, _repositories.QuestionFlagRepository);
            question.UnlinkRelatedEntities();

            _repositories.QuestionRepository.Delete(question);
            _repositories.QuestionRepository.Commit();

            var viewModel = new QuestionDeleteConfirmedViewModel();
            return viewModel;
        }

        public QuestionListViewModel BackToList()
        {
            var listPresenter = new QuestionListPresenter(_repositories);
            return listPresenter.Show();
        }
    }
}