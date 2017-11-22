using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
	public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
	{
		public CategoryRepository(IContext context)
			: base(context)
		{ }

		public virtual IList<Category> GetAll() => _context.Query<Category>().ToArray();

		public virtual Category TryGetByIdentifier(string identifier)
		{
			return _context.Query<Category>()
						   .Where(x => x.Identifier == identifier)
						   .SingleOrDefault();
		}

		public virtual Category TryGetCategoryByParentAndIdentifier(Category parentCategory, string identifier)
		{
			return _context.Query<Category>()
						   .Where(x => x.ParentCategory == parentCategory)
						   .Where(x => x.Identifier == identifier)
						   .SingleOrDefault();
		}

		public virtual IList<Category> GetRootCategories()
		{
			return _context.Query<Category>()
						   .Where(x => x.ParentCategory == null)
						   .ToArray();
		}
	}
}
