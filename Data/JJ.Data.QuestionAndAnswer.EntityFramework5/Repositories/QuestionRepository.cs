using JJ.Framework.Data;
using JJ.Framework.Data.EntityFramework5;
using JJ.Framework.Data.SqlClient;
using JJ.Data.QuestionAndAnswer.Sql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Data.QuestionAndAnswer.EntityFramework5.Helpers;

namespace JJ.Data.QuestionAndAnswer.EntityFramework5.Repositories
{
    public class QuestionRepository : JJ.Data.QuestionAndAnswer.DefaultRepositories.QuestionRepository
    {
        public QuestionRepository(IContext context)
            : base(context)
        {
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
