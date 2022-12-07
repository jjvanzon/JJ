using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionDeleteConfirmedPresenter
    {
        private readonly Repositories _repositories;
        private readonly SecurityAsserter _securityAsserter;
        private readonly string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionDeleteConfirmedPresenter(Repositories repositories, string authenticatedUserName)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
            _securityAsserter = new SecurityAsserter(repositories.UserRepository);
            _authenticatedUserName = authenticatedUserName;
        }

        public QuestionDeleteConfirmedViewModel Show(int id)
        {
            // Security
            _securityAsserter.Assert(_authenticatedUserName);

            // ToViewModel
            QuestionDeleteConfirmedViewModel viewModel = ViewModelHelper.CreateDeleteConfirmedViewModel(
                id,
                _repositories.UserRepository,
                _authenticatedUserName);

            return viewModel;
        }
    }
}