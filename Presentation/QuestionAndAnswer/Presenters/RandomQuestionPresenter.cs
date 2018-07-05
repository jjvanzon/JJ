using System.Collections.Generic;
using System.Linq;
using JJ.Business.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Aggregates;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.Helpers;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class RandomQuestionPresenter
	{
		private readonly IQuestionRepository _questionRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IQuestionFlagRepository _questionFlagRepository;
		private readonly IFlagStatusRepository _flagStatusRepository;
		private readonly IUserRepository _userRepository;
		private readonly string _authenticatedUserName;
		private readonly CategoryManager _categoryManager;

		/// <param name="authenticatedUserName">nullable</param>
		public RandomQuestionPresenter(
			IQuestionRepository questionRepository,
			ICategoryRepository categoryRepository,
			IQuestionFlagRepository questionFlagRepository,
			IFlagStatusRepository flagStatusRepository,
			IUserRepository userRepository,
			string authenticatedUserName)
		{
			_questionRepository = questionRepository ?? throw new NullException(() => questionRepository);
			_categoryRepository = categoryRepository ?? throw new NullException(() => categoryRepository);
			_questionFlagRepository = questionFlagRepository ?? throw new NullException(() => questionFlagRepository);
			_flagStatusRepository = flagStatusRepository ?? throw new NullException(() => flagStatusRepository);
			_userRepository = userRepository ?? throw new NullException(() => userRepository);

			_authenticatedUserName = authenticatedUserName;
			_categoryManager = new CategoryManager(_categoryRepository);
		}

		/// <param name="categoryIDs">nullable</param>
		public RandomQuestionViewModel Show(params int[] categoryIDs)
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
				var selector = new QuestionSelector(_questionRepository, selectedCategoryNodes);
				question = selector.TryGetRandomQuestion();
			}

			// Not Found
			if (question == null)
			{
			    throw new NotFoundException<Question>();
			}

			// Create ViewModel
			RandomQuestionViewModel viewModel = question.ToRandomQuestionViewModel(_userRepository, _authenticatedUserName);
			viewModel.Question.Links.Clear(); // Links reveal answer.
			viewModel.SelectedCategories = selectedCategoryBranches.Select(x => x.ToViewModel()).ToList();

			return viewModel;
		}

		public RandomQuestionViewModel ShowAnswer(RandomQuestionViewModel userInput)
		{
			// Check conditions
			if (userInput == null) throw new NullException(() => userInput);
			if (userInput.Question == null) throw new NullException(() => userInput.Question);
			userInput.NullCoalesce();

			// GetEntities
			Question question = _questionRepository.Get(userInput.Question.ID);
			User user = _userRepository.TryGetByUserName(_authenticatedUserName);
			QuestionFlag questionFlag = TryGetQuestionFlag(question, user);

			// ToViewModel
			RandomQuestionViewModel viewModel = question.ToRandomQuestionViewModel(_userRepository, _authenticatedUserName, questionFlag);

			// NonPersisted
			viewModel.UserAnswer = userInput.UserAnswer;
			viewModel.AnswerIsVisible = true;
			if (user != null)
			{
				viewModel.CurrentUserQuestionFlag.CanFlag = true;
			}
			if (userInput.SelectedCategories != null)
			{
				viewModel.SelectedCategories = userInput.SelectedCategories;
			}

			return viewModel;
		}

		public RandomQuestionViewModel HideAnswer(RandomQuestionViewModel userInput)
		{
			// Check conditions
			if (userInput == null) throw new NullException(() => userInput);
			if (userInput.Question == null) throw new NullException(() => userInput.Question);
			userInput.NullCoalesce();

			// GetEntities
			Question question = _questionRepository.Get(userInput.Question.ID);
			User user = _userRepository.TryGetByUserName(_authenticatedUserName);
			QuestionFlag questionFlag = TryGetQuestionFlag(question, user);

			// ToViewModel
			RandomQuestionViewModel viewModel = question.ToRandomQuestionViewModel(_userRepository, _authenticatedUserName, questionFlag);

			// NonPersisted
			viewModel.UserAnswer = userInput.UserAnswer;
			if (userInput.SelectedCategories != null)
			{
				viewModel.SelectedCategories = userInput.SelectedCategories;
			}

			// Links reveal answer
			viewModel.Question.Links.Clear(); 

			return viewModel;
		}

		public RandomQuestionViewModel Flag(RandomQuestionViewModel userInput)
		{
			// Check conditions
			if (userInput == null) throw new NullException(() => userInput);
			if (userInput.Question == null) throw new NullException(() => userInput.Question);
			if (userInput.CurrentUserQuestionFlag == null) throw new NullException(() => userInput.CurrentUserQuestionFlag);
			userInput.NullCoalesce();

			// GetEntities
			User user = _userRepository.GetByUserName(_authenticatedUserName);
			Question question = _questionRepository.Get(userInput.Question.ID);

			// Business
			var questionFlagger = new QuestionFlagger(user, _questionFlagRepository, _flagStatusRepository);
			QuestionFlag questionFlag = questionFlagger.FlagQuestion(question, userInput.CurrentUserQuestionFlag.Comment);
			_questionFlagRepository.Commit();

			// New Transaction: Get Entity
			question = _questionRepository.Get(userInput.Question.ID);

			// ToViewModel
			RandomQuestionViewModel viewModel = question.ToRandomQuestionViewModel(_userRepository, _authenticatedUserName, questionFlag);
			
			// NonPersisted
			viewModel.UserAnswer = userInput.UserAnswer;
			viewModel.AnswerIsVisible = userInput.AnswerIsVisible;
			viewModel.CurrentUserQuestionFlag.CanFlag = userInput.AnswerIsVisible;
			viewModel.SelectedCategories = userInput.SelectedCategories;

			return viewModel;
		}

		// TODO: Program this and delegate to it everywhere.
		//private void CopyNonPersistedProperties(RandomQuestionViewModel source, RandomQuestionViewModel dest)

		public RandomQuestionViewModel Unflag(RandomQuestionViewModel userInput)
		{
			// Check conditions
			if (userInput == null) throw new NullException(() => userInput);
			if (userInput.Question == null) throw new NullException(() => userInput.Question);
			if (userInput.CurrentUserQuestionFlag == null) throw new NullException(() => userInput.CurrentUserQuestionFlag);
			userInput.NullCoalesce();

			// Get entities
			User user = _userRepository.GetByUserName(_authenticatedUserName);
			Question question = _questionRepository.Get(userInput.Question.ID);

			// Call business logic
			var questionFlagger = new QuestionFlagger(user, _questionFlagRepository, _flagStatusRepository);
			questionFlagger.UnflagQuestion(question);
			_questionFlagRepository.Commit();

			// New Transaction: Get Entity
			question = _questionRepository.Get(userInput.Question.ID);

			// Create new view model
			RandomQuestionViewModel viewModel = question.ToRandomQuestionViewModel(_userRepository, _authenticatedUserName);

			// Set non-persisted properties
			viewModel.UserAnswer = userInput.UserAnswer;
			viewModel.AnswerIsVisible = userInput.AnswerIsVisible;
			viewModel.CurrentUserQuestionFlag.CanFlag = userInput.AnswerIsVisible;

			return viewModel;
		}

		public RandomQuestionViewModel SetLanguage(RandomQuestionViewModel userInput, string cultureName)
		{
			// Check conditions
			if (userInput == null) throw new NullException(() => userInput);
			userInput.NullCoalesce();

			// Business logic
			CultureHelper.SetCulture(cultureName);

			// Get entities
			Question question = _questionRepository.Get(userInput.Question.ID);
			User user = _userRepository.TryGetByUserName(_authenticatedUserName);
			QuestionFlag questionFlag = TryGetQuestionFlag(question, user);

			// Create new view model
			RandomQuestionViewModel viewModel = question.ToRandomQuestionViewModel(_userRepository, _authenticatedUserName, questionFlag);

			// Set non-persisted properties
			viewModel.UserAnswer = userInput.UserAnswer;
			if (userInput.SelectedCategories != null)
			{
				viewModel.SelectedCategories = userInput.SelectedCategories;
			}

			// Links reveal answer.
			viewModel.Question.Links.Clear();

			return viewModel;
		}

		public RandomQuestionViewModel NextQuestion(RandomQuestionViewModel userInput)
		{
			if (userInput == null) throw new NullException(() => userInput);

			int[] categoryIDs = userInput.SelectedCategories.Select(x => x.ID).ToArray();
			RandomQuestionViewModel viewModel = Show(categoryIDs);
			return viewModel;
		}

		private QuestionFlag TryGetQuestionFlag(Question question, User user)
		{
			if (question == null) throw new NullException(() => question);
			if (user == null)
			{
				return null;
			}

			var questionFlagger = new QuestionFlagger(user, _questionFlagRepository, _flagStatusRepository);
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
