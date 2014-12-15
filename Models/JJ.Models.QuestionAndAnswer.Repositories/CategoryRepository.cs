using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;

namespace JJ.Models.QuestionAndAnswer.Repositories
{
    public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        public CategoryRepository(IContext context)
            : base(context)
        { }

        public Category TryGetByIdentifier(string identifier)
        {
            return _context.Query<Category>()
                           .Where(x => x.Identifier == identifier)
                           .SingleOrDefault();
        }

        public Category TryGetCategoryByParentAndIdentifier(Category parentCategory, string identifier)
        {
            if (parentCategory == null) throw new ArgumentNullException("parentCategory");

            return _context.Query<Category>()
                           .Where(x => x.ParentCategory == parentCategory)
                           .Where(x => x.Identifier == identifier)
                           .SingleOrDefault();
        }

        public IList<Category> GetRootCategories()
        {
            return _context.Query<Category>()
                           .Where(x => x.ParentCategory == null)
                           .ToArray();
        }
    }
}
