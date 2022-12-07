//using JJ.Framework.Persistence;
//using JJ.Models.QuestionAndAnswer.SqlClient.Sql;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace JJ.Models.QuestionAndAnswer.SqlClient.Repositories
//{
//    public class QuestionRepository : JJ.Models.QuestionAndAnswer.DefaultRepositories.QuestionRepository
//    {
//        private SqlExecutor _sqlExecutor;
//        private JJ.Framework.Persistence.SqlClient.SqlExecutor _sqlExecutor2;

//        public QuestionRepository(IContext context)
//            : base(context)
//        {
//            string sqlConnectionString = context.Location;
//            _sqlExecutor = new SqlExecutor(sqlConnectionString);
//        }

//        public override Question TryGetRandomQuestion()
//        {
//            int? randomID = _sqlExecutor.Question_TryGetRandom();
//            if (randomID.HasValue)
//            {
//                return Get(randomID.Value);
//            }
//            else
//            {
//                return null;
//            }
//        }
//    }
//}
