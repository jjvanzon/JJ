using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
{
    public class QuestionCategoryRepository : IQuestionCategoryRepository
    {
        private IContext _context;

        public QuestionCategoryRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public QuestionCategory Create()
        {
            return _context.Create<QuestionCategory>();
        }

        public void Delete(QuestionCategory questionCategory)
        {
            _context.Delete(questionCategory);
        }
    }
}
