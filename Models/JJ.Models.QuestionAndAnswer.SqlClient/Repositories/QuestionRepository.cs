using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer.SqlClient.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.SqlClient.Repositories
{
    public class QuestionRepository : JJ.Models.QuestionAndAnswer.Repositories.QuestionRepository
    {
        private SqlExecutor _sqlExecutor;

        public QuestionRepository(IContext context)
            : base(context)
        {
            string sqlConnectionString = context.Location;
            _sqlExecutor = new SqlExecutor(sqlConnectionString);
        }

        public override Question TryGetRandomQuestion()
        {
            int? randomID = _sqlExecutor.TryGetRandomQuestionID();
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
