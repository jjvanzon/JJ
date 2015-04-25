using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
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
            if (parentCategory == null) throw new NullException(() => parentCategory);

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
