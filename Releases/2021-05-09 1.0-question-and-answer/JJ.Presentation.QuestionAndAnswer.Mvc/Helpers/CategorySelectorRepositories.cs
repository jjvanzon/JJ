using System;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Helpers
{
    [Obsolete("Use two variables ICategoryRepository and IUserRepository instead.")]
	internal class CategorySelectorRepositories
	{
		public ICategoryRepository CategoryRepository { get; }
		public IUserRepository UserRepository { get; }

		public CategorySelectorRepositories(
			ICategoryRepository categoryRepository,
			IUserRepository userRepository)
		{
			CategoryRepository = categoryRepository ?? throw new NullException(() => categoryRepository);
			UserRepository = userRepository ?? throw new NullException(() => userRepository);
		}
	}
}