using System.Collections.Generic;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces
{
	public interface ICategoryRepository : IRepository<Category, int>
	{
		IList<Category> GetAll();

		Category TryGetByIdentifier(string identifier);

		/// <summary> TODO: Refactor so that it takes parentCategoryID instead. </summary>
		Category TryGetCategoryByParentAndIdentifier(Category parentCategory, string identifier);
		IList<Category> GetRootCategories();
	}
}
