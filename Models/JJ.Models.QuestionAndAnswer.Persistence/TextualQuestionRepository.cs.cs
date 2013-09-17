using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.SQL;
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

        private SqlExecutor _sqlExecutor;

        public TextualQuestionRepository(IContext context, string sqlConnectionString)
        {
            if (context == null) throw new ArgumentNullException("context");

            _context = context;

            _sqlExecutor = new SqlExecutor(sqlConnectionString);
        }

        public IEnumerable<TextualQuestion> GetAll()
        {
            return _context.Query<TextualQuestion>().ToArray();
        }

        public TextualQuestion Get(int id)
        {
            return _context.Get<TextualQuestion>(id);
        }

        public TextualQuestion GetRandomTextualQuestion()
        {
            int randomID = _sqlExecutor.GetRandomTextualQuestionID();
            return Get(randomID);
        }
    }
}
