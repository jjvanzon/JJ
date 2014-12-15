using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Models.QuestionAndAnswer.Repositories;
using JJ.Business.QuestionAndAnswer;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class RandomQuestionPresenter
    {
        private IQuestionRepository _questionRepository;
        private ICategoryRepository _categoryRepository;
        private IQuestionFlagRepository _questionFlagRepository;
        private IFlagStatusRepository _flagStatusRepository;
        private IUserRepository _userRepository;

        private CategoryManager _categoryManager;

        public RandomQuestionPresenter(
            IQuestionRepository questionRepository,
            ICategoryRepository categoryRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository,
            IUserRepository userRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _questionFlagRepository = questionFlagRepository;
            _flagStatusRepository = flagStatusRepository;
            _userRepository = userRepository;

            _categoryManager = new CategoryManager(_categoryRepository);
        }

        /// <summary>
        /// Can return RandomQuestionViewModel or QuestionNotFoundViewModel.
        /// </summary>
        public object Show(params int[] categoryIDs)
        {
            categoryIDs = categoryIDs ?? new int[0];

            // Get Categories
            IList<Category> selectedCategoryBranches = GetCategories(categoryIDs);
            IList<Category> selectedCategoryNodes = _categoryManager.SelectNodesRecursive(selectedCategoryBranches);

            // Get Random Question
            Question question;
            if (selectedCategoryNodes.Count == 0)
            {
                question = _questionRepository.TryGetRandomQuestion();
            }
            else
            {
                QuestionSelector selector = new QuestionSelector(_questionRepository, selectedCategoryNodes);
                question = selector.TryGetRandomQuestion();
            }

            // Not Found
            if (question == null)
            {
                return new QuestionNotFoundViewModel();
            }

            // Create ViewModel
            RandomQuestionViewModel viewModel = question.ToRandomQuestionViewModel();
            viewModel.AnswerIsVisible = false; // TODO: Do I need to set this default?
            viewModel.Question.Answer = null; // TODO: Do I need to set this default?
            viewModel.Question.Links.Clear(); // Links reveal answer.
            viewModel.SelectedCategories = selectedCategoryBranches.Select(x => x.ToViewModel()).ToList();

            return viewModel;
        }

        /// <summary>
        /// Can return RandomQuestionViewModel or QuestionNotFoundViewModel.
        /// </summary>
        public object ShowAnswer(RandomQuestionViewModel viewModel, string userName)
        {
            // Check conditions
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (viewModel.Question == null) throw new ArgumentNullException("viewModel.Question");

            // Get entities
            Question question = _questionRepository.TryGet(viewModel.Question.ID);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter();
                return presenter2.Show();
            }
            User user = _userRepository.TryGetByUserName(userName);
            QuestionFlag questionFlag = TryGetQuestionFlag(question, user);

            // Create new view model
            RandomQuestionViewModel viewModel2 = question.ToRandomQuestionViewModel(questionFlag);

            // Set non-persisted properties
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = true;
            if (user != null)
            {
                viewModel2.CurrentUserQuestionFlag.CanFlag = true;
            }
            if (viewModel.SelectedCategories != null)
            {
                viewModel2.SelectedCategories = viewModel.SelectedCategories;
            }

            return viewModel2;
        }

        /// <summary>
        /// Can return RandomQuestionViewModel or QuestionNotFoundViewModel.
        /// </summary>
        public object HideAnswer(RandomQuestionViewModel viewModel, string userName)
        {
            // Check conditions
            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }
            if (viewModel.Question == null) { throw new ArgumentNullException("viewModel.Question"); }

            // Get entities
            Question question = _questionRepository.TryGet(viewModel.Question.ID);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter();
                return presenter2.Show();
            }
            User user = _userRepository.TryGetByUserName(userName);
            QuestionFlag questionFlag = TryGetQuestionFlag(question, user);

            // Create new view model
            RandomQuestionViewModel viewModel2 = question.ToRandomQuestionViewModel(questionFlag);

            // Set non-persisted properties
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = false; // TODO: Do I have to set this default.
            viewModel2.CurrentUserQuestionFlag.CanFlag = false; // TODO: Do I have to set this default.
            if (viewModel.SelectedCategories != null)
            {
                viewModel2.SelectedCategories = viewModel.SelectedCategories;
            }

            // Links reveal answer.
            viewModel2.Question.Links.Clear(); 

            return viewModel2;
        }

        /// <summary> Can return RandomQuestionViewModel, QuestionNotFoundViewModel or NotAuthenticatedViewModel. </summary>
        public object Flag(RandomQuestionViewModel viewModel, string userName)
        {
            // Check conditions
            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }
            if (viewModel.Question == null) { throw new ArgumentNullException("viewModel.Question"); }
            if (viewModel.CurrentUserQuestionFlag == null) { throw new ArgumentNullException("viewModel.CurrentUserQuestionFlag"); }

            // Get entities
            User user = _userRepository.GetByUserName(userName);
            Question question = _questionRepository.TryGet(viewModel.Question.ID);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter();
                return presenter2.Show();
            }

            // Call business logic
            var questionFlagger = new QuestionFlagger(_questionFlagRepository, _flagStatusRepository, user);
            QuestionFlag questionFlag = questionFlagger.FlagQuestion(question, viewModel.CurrentUserQuestionFlag.Comment);
            _questionFlagRepository.Commit();

            // Create new view model
            RandomQuestionViewModel viewModel2 = question.ToRandomQuestionViewModel(questionFlag);
            
            // Set non-persisted properties
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = viewModel.AnswerIsVisible;
            viewModel2.CurrentUserQuestionFlag.CanFlag = viewModel.AnswerIsVisible;

            return viewModel2;
        }

        /// <summary> Can return RandomQuestionViewModel, QuestionNotFoundViewModel or NotAuthenticatedViewModel. </summary>
        public object Unflag(RandomQuestionViewModel viewModel, string userName)
        {
            // Check conditions
            if (viewModel == null) { throw new ArgumentNullException("viewModel"); }
            if (viewModel.Question == null) { throw new ArgumentNullException("viewModel.Question"); }
            if (viewModel.CurrentUserQuestionFlag == null) { throw new ArgumentNullException("viewModel.CurrentUserQuestionFlag"); }

            // Get entities
            User user = _userRepository.GetByUserName(userName);
            Question question = _questionRepository.TryGet(viewModel.Question.ID);
            if (question == null)
            {
                var presenter2 = new QuestionNotFoundPresenter();
                return presenter2.Show();
            }

            // Call business logic
            var questionFlagger = new QuestionFlagger(_questionFlagRepository, _flagStatusRepository, user);
            questionFlagger.UnflagQuestion(question);
            _questionFlagRepository.Commit();

            // Create new view model
            RandomQuestionViewModel viewModel2 = question.ToRandomQuestionViewModel();

            // Set non-persisted properties
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = viewModel.AnswerIsVisible;
            viewModel2.CurrentUserQuestionFlag.CanFlag = viewModel.AnswerIsVisible;

            return viewModel2;
        }

        private QuestionFlag TryGetQuestionFlag(Question question, User user)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (user == null)
            {
                return null;
            }

            var questionFlagger = new QuestionFlagger(_questionFlagRepository, _flagStatusRepository, user);
            QuestionFlag questionFlag = questionFlagger.TryGetFlag(question);

            return questionFlag;
        }

        private List<Category> GetCategories(int[] ids)
        {
            var list = new List<Category>();

            foreach (int id in ids)
            {
                Category category = _categoryRepository.Get(id);
                list.Add(category);
            }

            return list;
        }
    }
}
