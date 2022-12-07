using JJ.Business.QuestionAndAnswer.Cascading;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionConfirmDeletePresenter
    {
        private readonly Repositories _repositories;
        private readonly SecurityAsserter _securityAsserter;
        private readonly string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionConfirmDeletePresenter(Repositories repositories, string authenticatedUserName)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
            _securityAsserter = new SecurityAsserter(repositories.UserRepository);
            _authenticatedUserName = authenticatedUserName;
        }

        public QuestionConfirmDeleteViewModel Show(int id)
        {
            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // GetEntity
            Question question = _repositories.QuestionRepository.Get(id);

            // ToViewModel
            QuestionConfirmDeleteViewModel viewModel = question.ToConfirmDeleteViewModel(_repositories.UserRepository, _authenticatedUserName);

            return viewModel;
        }

        public QuestionDeleteConfirmedViewModel Confirm(int id)
        {
            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // GetEntity
            Question question = _repositories.QuestionRepository.Get(id);

            // Business
            question.DeleteRelatedEntities(
                _repositories.AnswerRepository,
                _repositories.QuestionCategoryRepository,
                _repositories.QuestionLinkRepository,
                _repositories.QuestionFlagRepository);

            question.UnlinkRelatedEntities();

            _repositories.QuestionRepository.Delete(question);

            // Commit
            _repositories.QuestionRepository.Commit();

            // Redirect
            var presenter2 = new QuestionDeleteConfirmedPresenter(_repositories, _authenticatedUserName);
            return presenter2.Show(id);
        }
    }
}