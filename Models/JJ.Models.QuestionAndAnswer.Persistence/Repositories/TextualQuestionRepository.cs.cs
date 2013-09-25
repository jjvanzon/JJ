using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;

namespace JJ.Models.QuestionAndAnswer.Persistence.Repositories
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

        public TextualQuestion TryGet(int id)
        {
            return _context.TryGet<TextualQuestion>(id);
        }

        public TextualQuestion Get(int id)
        {
            return _context.Get<TextualQuestion>(id);
        }

        public TextualQuestion TryGetRandomTextualQuestion()
        {
            int? randomID = _sqlExecutor.TryGetRandomTextualQuestionID();
            if (randomID.HasValue)
            {
                return Get(randomID.Value);
            }
            else
            {
                return null;
            }
        }

        public TextualQuestion Create()
        {
            TextualQuestion entity = _context.Create<TextualQuestion>();
            return entity;
        }

        public IEnumerable<TextualQuestion> GetBySource(int sourceID)
        {
            return _context.Query<TextualQuestion>().Where(x => x.Source.ID == sourceID);
        }

        public void Delete(TextualQuestion textualQuestion)
        {
            _context.Delete(textualQuestion);
        }

        public void Commit()
        {
            _context.Commit();
        }
    }
}
