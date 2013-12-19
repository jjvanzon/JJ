using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionFlagPresenter
    {
        private IUserRepository _userRepository;
        private IQuestionRepository _questionRepository;
        private IQuestionFlagRepository _questionFlagRepository;

        public QuestionFlagPresenter(IUserRepository userRepository, IQuestionRepository questionRepository, IQuestionFlagRepository questionFlagRepository)
        {
            if (userRepository == null) { throw new ArgumentNullException("userRepository"); }
            if (questionRepository == null) { throw new ArgumentNullException("questionRepository"); }
            if (questionFlagRepository == null) { throw new ArgumentNullException("questionFlagRepository"); }

            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _questionFlagRepository = questionFlagRepository;
        }

        /// <summary>
        /// Can return QuestionFlagViewModel, NotAuthenticatedViewModel.
        /// </summary>
        /// <param name="loginViewModel">If loginViewModel.IsLoggedIn is true, the user is considered authenticated.</param>
        public object Show(int questionID, LoginViewModel loginViewModel)
        {
            if (loginViewModel == null) { throw new ArgumentNullException("loginViewModel"); }

            if (!loginViewModel.IsLoggedIn)
            {
                return new NotAuthenticatedViewModel();
            }
            
            // TODO: Delegate to the business layer.

            User user = _userRepository.GetByUserName(loginViewModel.UserName);
            Question question = _questionRepository.Get(questionID);
            QuestionFlag questionFlag = _questionFlagRepository.TryGetByCriteria(questionID, user.ID);

            bool isFlagged = false;
            string comment = null;
            if (questionFlag != null)
            {
                isFlagged = true;
                comment = questionFlag.Comment;
            }

            var viewModel = new QuestionFlagViewModel
            {
                Login = loginViewModel,
                IsFlagged = isFlagged,
                Comment = comment,
                Question = question.ToViewModel()
            };

            return viewModel;
        }

        /// <summary>
        /// Can return QuestionFlagViewModel, NotAuthenticatedViewModel.
        /// </summary>
        /// <param name="loginViewModel">If loginViewModel.IsLoggedIn is true, the user is considered authenticated.</param>
        public object Flag(QuestionFlagViewModel viewModel)
        {
            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }
            if (viewModel.Login == null) { throw new ArgumentNullException("viewModel.Login"); }
            if (viewModel.Question == null) { throw new ArgumentNullException("viewModel.Question"); }

            if (!viewModel.Login.IsLoggedIn)
            {
                return new NotAuthenticatedViewModel();
            }

            User user = _userRepository.GetByUserName(viewModel.Login.UserName);
            Question question = _questionRepository.Get(viewModel.Question.ID);
            QuestionFlag questionFlag = _questionFlagRepository.TryGetByCriteria(viewModel.Question.ID, user.ID);

            if (questionFlag == null)
            {
                questionFlag = _questionFlagRepository.Create();
                questionFlag.LinkTo(question);
                questionFlag.LinkToFlaggedByUser(user);
            }

            questionFlag.LinkToLastModifiedByUser(user);
            questionFlag.DateTime = DateTime.Now;
            questionFlag.Comment = viewModel.Comment;

            _questionFlagRepository.Commit();

            return new QuestionFlagViewModel
            {
                Login = viewModel.Login,
                IsFlagged = true,
                Comment = viewModel.Comment,
                Question = question.ToViewModel()
            };
        }

        /// <summary>
        /// Can return QuestionFlagViewModel, NotAuthenticatedViewModel.
        /// </summary>
        /// <param name="loginViewModel">If loginViewModel.IsLoggedIn is true, the user is considered authenticated.</param>
        public object UnFlag(QuestionFlagViewModel viewModel)
        {
            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }
            if (viewModel.Login == null) { throw new ArgumentNullException("viewModel.Login"); }
            if (viewModel.Question == null) { throw new ArgumentNullException("viewModel.Question"); }

            if (!viewModel.Login.IsLoggedIn)
            {
                return new NotAuthenticatedViewModel();
            }

            User user = _userRepository.GetByUserName(viewModel.Login.UserName);
            Question question = _questionRepository.Get(viewModel.Question.ID);
            QuestionFlag questionFlag = _questionFlagRepository.TryGetByCriteria(viewModel.Question.ID, user.ID);

            if (questionFlag != null)
            {
                questionFlag.Unlink(question);
                questionFlag.UnlinkFlaggedByUser(user);
                _questionFlagRepository.Delete(questionFlag);
            }

            _questionFlagRepository.Commit();

            return new QuestionFlagViewModel
            {
                Login = viewModel.Login,
                IsFlagged = false,
                Comment = viewModel.Comment,
                Question = question.ToViewModel()
            };
        }
    }
}
