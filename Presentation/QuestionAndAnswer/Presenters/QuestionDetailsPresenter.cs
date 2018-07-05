using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class QuestionDetailsPresenter
    {
        private readonly Repositories _repositories;
        private readonly string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public QuestionDetailsPresenter(Repositories repositories, string authenticatedUserName)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
            _authenticatedUserName = authenticatedUserName;
        }

        public object Show(int id)
        {
            // GetEntity
            Question question = _repositories.QuestionRepository.Get(id);

            // ToViewModel
            QuestionDetailsViewModel viewModel = question.ToDetailsViewModel(_repositories.UserRepository, _authenticatedUserName);

            return viewModel;
        }
    }
}