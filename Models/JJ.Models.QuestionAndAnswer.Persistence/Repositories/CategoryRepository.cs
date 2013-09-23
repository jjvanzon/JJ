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
    }
}
