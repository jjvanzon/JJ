using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        [UsedImplicitly]
        public CategoryRepository(IContext context) : base(context) { }

        public virtual IList<Category> GetAll() => _context.Query<Category>().ToArray();

        public virtual Category TryGetByIdentifier(string identifier)
            => _context.Query<Category>().SingleOrDefault(x => x.Identifier == identifier);

        public virtual IList<Category> TryGetManyByIdentifier(string identifier)
            => _context.Query<Category>()
                       .Where(x => x.Identifier == identifier)
                       .ToArray();

        public virtual IList<Category> GetRootCategories()
            => _context.Query<Category>()
                       .Where(x => x.ParentCategory == null)
                       .ToArray();
    }
}