using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public class TextualAnswerRepository : ITextualAnswerRepository
    {
        private IContext _context;

        public TextualAnswerRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public TextualAnswer Create()
        {
            return _context.Create<TextualAnswer>();
        }

        public void Delete(TextualAnswer textualAnswer)
        {
            _context.Delete(textualAnswer);
        }
    }
}
