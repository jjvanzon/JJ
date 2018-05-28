using System.Collections.Generic;
using System.Linq;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
	public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
	{
		public CategoryRepository(IContext context)
			: base(context) { }

		public virtual IList<Category> GetAll() => _context.Query<Category>().ToArray();

		public virtual Category TryGetByIdentifier(string identifier)
		{
			return _context.Query<Category>()
			               .Where(x => x.Identifier == identifier)
			               .SingleOrDefault();
		}

		public virtual IList<Category> TryGetManyByIdentifier(string identifier)
		{
			return _context.Query<Category>()
			               .Where(x => x.Identifier == identifier)
			               .ToArray();
		}

		public virtual IList<Category> GetRootCategories()
		{
			return _context.Query<Category>()
			               .Where(x => x.ParentCategory == null)
			               .ToArray();
		}
	}
}