using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private IContext _context;

        public CategoryRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public Category Get(int id)
        {
            return _context.Get<Category>(id);
        }

        public Category TryGetByIdentifier(string identifier)
        {
            return _context.Query<Category>()
                           .Where(x => x.Identifier == identifier)
                           .SingleOrDefault();
        }

        /*public Category TryGetByIdentifier(string identifier)
        {
            Category category;

            category = _context.QueryUncommitted<Category>()
                               .Where(x => x.Identifier == identifier)
                               .SingleOrDefault();

            if (category == null)
            {
                category = _context.Query<Category>()
                           .Where(x => x.Identifier == identifier)
                           .SingleOrDefault();
            }

            return category;
        }*/

        public Category Create()
        {
            return _context.Create<Category>();
        }

        /*public Category TryGetByParentAndIdentifier(Category parentCategory, string identifier)
        {
            if (parentCategory == null) throw new ArgumentNullException("parentCategory");

            Category category;

            category = _context.QueryUncommitted<Category>()
                               .Where(x => x.ParentCategory == parentCategory)
                               .Where(x => x.Identifier == identifier)
                               .SingleOrDefault();

            if (category == null)
            {
                category = _context.Query<Category>()
                                   .Where(x => x.ParentCategory == parentCategory)
                                   .Where(x => x.Identifier == identifier)
                                   .SingleOrDefault();
            }

            return category;
        }*/

        public Category TryGetCategoryByParentAndIdentifier(Category parentCategory, string identifier)
        {
            if (parentCategory == null) throw new ArgumentNullException("parentCategory");

            return _context.Query<Category>()
                           .Where(x => x.ParentCategory == parentCategory)
                           .Where(x => x.Identifier == identifier)
                           .SingleOrDefault();
        }
    }
}
