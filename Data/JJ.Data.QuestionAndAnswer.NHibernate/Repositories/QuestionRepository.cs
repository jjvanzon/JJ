using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;
using JJ.Data.QuestionAndAnswer.Sql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Data.QuestionAndAnswer.NHibernate.Helpers;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
    public class QuestionRepository : JJ.Data.QuestionAndAnswer.DefaultRepositories.QuestionRepository
    {
        private new NHibernateContext _context;

        public QuestionRepository(IContext context)
            : base(context)
        { }

        public override IList<Question> GetPage(int firstIndex, int count)
        {
            QuestionAndAnswerSqlExecutor sqlExecutor = SqlExecutorHelper.CreateQuestionAndAnswerSqlExecutor(_context);

            IList<Question> list = new List<Question>(count);

            IList<int> ids = sqlExecutor.Question_GetPageOfIDs(firstIndex, count).ToArray();
            foreach (int id in ids)
            {
                Question entity = Get(id);
                list.Add(entity);
            }

            return list;
        }

        public override Question TryGetRandomQuestion()
        {
            QuestionAndAnswerSqlExecutor sqlExecutor = SqlExecutorHelper.CreateQuestionAndAnswerSqlExecutor(_context);

            int? randomID = sqlExecutor.Question_TryGetRandomID();
            if (randomID.HasValue)
            {
                return Get(randomID.Value);
            }
            else
            {
                return null;
            }
        }

        public override int CountAll()
        {
            QuestionAndAnswerSqlExecutor sqlExecutor = SqlExecutorHelper.CreateQuestionAndAnswerSqlExecutor(_context);
            return sqlExecutor.Question_CountAll();
        }
    }
}
