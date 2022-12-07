using System.Collections.Generic;
using JetBrains.Annotations;
using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
    public class CategoryRepository : DefaultRepositories.CategoryRepository
    {
        private new readonly NHibernateContext _context;

        [UsedImplicitly]
        public CategoryRepository(IContext context) : base(context) => _context = (NHibernateContext)context;

        public override IList<Category> GetAll() => _context.Session.QueryOver<Category>().List();

        public override Category TryGetByIdentifier(string identifier)
            => _context.Session.QueryOver<Category>()
                       .Where(x => x.Identifier == identifier)
                       .SingleOrDefault();

        public override IList<Category> TryGetManyByIdentifier(string identifier)
            => _context.Session.QueryOver<Category>()
                       .Where(x => x.Identifier == identifier)
                       .List();

        public override IList<Category> GetRootCategories()
            => _context.Session.QueryOver<Category>()
                       .Where(x => x.ParentCategory == null)
                       .List();
    }
}