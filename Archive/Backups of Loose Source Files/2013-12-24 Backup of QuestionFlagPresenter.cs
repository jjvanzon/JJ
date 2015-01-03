//using JJ.Apps.QuestionAndAnswer.ViewModels;
//using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;
//using JJ.Models.QuestionAndAnswer;
//using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
//using JJ.Business.QuestionAndAnswer.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using JJ.Business.QuestionAndAnswer;

//namespace JJ.Apps.QuestionAndAnswer.Presenters
//{
//    public class QuestionFlagPresenter
//    {
//        private IUserRepository _userRepository;
//        private IQuestionRepository _questionRepository;
//        private IQuestionFlagRepository _questionFlagRepository;

//        public QuestionFlagPresenter(IUserRepository userRepository, IQuestionRepository questionRepository, IQuestionFlagRepository questionFlagRepository)
//        {
//            if (userRepository == null) { throw new ArgumentNullException("userRepository"); }
//            if (questionRepository == null) { throw new ArgumentNullException("questionRepository"); }
//            if (questionFlagRepository == null) { throw new ArgumentNullException("questionFlagRepository"); }

//            _userRepository = userRepository;
//            _questionRepository = questionRepository;
//            _questionFlagRepository = questionFlagRepository;
//        }

//        /// <summary>
//        /// Can return QuestionFlagViewModel, NotAuthenticatedViewModel.
//        /// </summary>
//        /// <param name="loginViewModel">If loginViewModel.IsLoggedIn is true, the user is considered authenticated.</param>
//        public object Show(int questionID, LoginViewModel loginViewModel)
//        {
//            if (loginViewModel == null) { throw new ArgumentNullException("loginViewModel"); }

//            if (!loginViewModel.IsLoggedIn)
//            {
//                return new NotAuthenticatedViewModel();
//            }

//            User user = _userRepository.GetByUserName(loginViewModel.UserName);
//            Question question = _questionRepository.Get(questionID);

//            var questionFlagger = new QuestionFlagger(_questionFlagRepository, user);
//            QuestionFlag questionFlag = questionFlagger.TryGetFlag(question);

//            if (questionFlag == null)
//            {
//                return new QuestionFlagViewModel();
//            }
//            else
//            {
//                return questionFlag.ToViewModel();
//            }
//        }
//    }
//}
