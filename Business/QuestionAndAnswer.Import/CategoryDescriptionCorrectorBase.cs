using JJ.Framework.Exceptions;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;

namespace JJ.Business.QuestionAndAnswer.Import
{
	public abstract class CategoryDescriptionCorrectorBase
	{
		private ICategoryRepository _categoryRepository;

		public CategoryDescriptionCorrectorBase(ICategoryRepository categoryRepository)
		{
			if (categoryRepository == null) throw new NullException(() => categoryRepository);

			_categoryRepository = categoryRepository;
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
