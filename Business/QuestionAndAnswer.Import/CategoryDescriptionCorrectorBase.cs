using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.Import
{
	public abstract class CategoryDescriptionCorrectorBase
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryDescriptionCorrectorBase(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository ?? throw new NullException(() => categoryRepository);
		}

		public abstract void Execute();

		protected void CorrectCategoryDescription(string identifier, string description)
		{
			Category category = _categoryRepository.TryGetByIdentifier(identifier);
			if (category == null)
			{
				return;
			}
			category.Description = description;
		}
	}
}
