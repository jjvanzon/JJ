using System.Collections.Generic;
using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;
using JJ.Framework.Exceptions;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
	public class CategoryRepository : DefaultRepositories.CategoryRepository
	{
		private new readonly NHibernateContext _context;

		public CategoryRepository(IContext context)
			: base(context)
		{
			_context = (NHibernateContext)context;
		}

		public override IList<Category> GetAll() => _context.Session.QueryOver<Category>().List();

		public override Category TryGetByIdentifier(string identifier)
		{
			return _context.Session.QueryOver<Category>()
								   .Where(x => x.Identifier == identifier)
								   .SingleOrDefault();
		}

		public override Category TryGetCategoryByParentAndIdentifier(Category parentCategory, string identifier)
		{
			if (parentCategory == null) throw new NullException(() => parentCategory);

			int parentCategoryID = parentCategory.ID;

			Category c = null;
			Category pc = null;

			return _context.Session.QueryOver(() => c)
								   .JoinAlias(() => c.ParentCategory, () => pc)
								   .Where(() => c.Identifier == identifier)
								   .Where(() => pc.ID == parentCategoryID)
								   .SingleOrDefault();
		}

		public override IList<Category> GetRootCategories()
		{
			return _context.Session.QueryOver<Category>()
								   .Where(x => x.ParentCategory == null)
								   .List();
		}
	}
}
