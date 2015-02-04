using JJ.Framework.Persistence;
using JJ.Framework.Persistence.EntityFramework5;
using JJ.Framework.Persistence.SqlClient;
using JJ.Models.QuestionAndAnswer.Sql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.EntityFramework5.Repositories
{
    public class QuestionRepository : JJ.Models.QuestionAndAnswer.DefaultRepositories.QuestionRepository
    {
        private QuestionAndAnswerSqlExecutor _sqlExecutor;

        public QuestionRepository(IContext context)
            : base(context)
        {
            EntityFramework5Context castedContext = (EntityFramework5Context)context;
            SqlConnection sqlConnection = castedContext.Context.Database.Connection as SqlConnection;
            if (sqlConnection == null)
            {
                throw new Exception("EntityFramework5Context.Context.Database.Connection must be an SqlConnection.");
            }
            _sqlExecutor = new QuestionAndAnswerSqlExecutor(new SqlExecutor(sqlConnection));
        }

        public override Question TryGetRandomQuestion()
        {
            int? randomID = _sqlExecutor.Question_TryGetRandomID();
            if (randomID.HasValue)
            {
                return Get(randomID.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
