using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence
{
    public class TextualQuestionRepository : ITextualQuestionRepository
    {
        private IContext _context;

        public TextualQuestionRepository(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;
        }

        public IEnumerable<EntityWrapper<TextualQuestion>> GetAll()
        {
            return _context.Query<TextualQuestion>().ToArray();
        }

        public EntityWrapper<TextualQuestion> Get(int id)
        {
            return _context.GetEntity<TextualQuestion>(id);
        }
    }
}
