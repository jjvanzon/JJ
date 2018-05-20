using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Helpers
{
	internal class CategorySelectorRepositories
	{
		public ICategoryRepository CategoryRepository { get; }
		public IQuestionRepository QuestionRepository { get; }
		public IQuestionFlagRepository QuestionFlagRepository { get; }
		public IFlagStatusRepository FlagStatusRepository { get; }
		public IUserRepository UserRepository { get; }

		public CategorySelectorRepositories(
			ICategoryRepository categoryRepository,
			IQuestionRepository questionRepository,
			IQuestionFlagRepository questionFlagRepository,
			IFlagStatusRepository flagStatusRepository,
			IUserRepository userRepository)
		{
			CategoryRepository = categoryRepository ?? throw new NullException(() => categoryRepository);
			QuestionRepository = questionRepository ?? throw new NullException(() => questionRepository);
			QuestionFlagRepository = questionFlagRepository ?? throw new NullException(() => questionFlagRepository);
			FlagStatusRepository = flagStatusRepository ?? throw new NullException(() => flagStatusRepository);
			UserRepository = userRepository ?? throw new NullException(() => userRepository);
		}
	}
}