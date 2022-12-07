//using JJ.Framework.Common;
//using JJ.Framework.Reflection;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace JJ.Models.QuestionAndAnswer.SqlClient.Sql
//{
//    internal class SqlExecutor_Old
//    {
//        private string _connectionString;

//        public SqlExecutor_Old(string connectionString)
//        {
//            if (String.IsNullOrEmpty(connectionString)) throw new NullException(() => connectionString);

//            _connectionString = connectionString;
//        }

//        public IList<int> Question_GetPageOfIDs(int firstIndex, int count)
//        {
//            IList<int> list = new List<int>(count);

//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                connection.Open();
//                string sql = GetSql(SqlEnum.Question_GetPageOfIDs);

//                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
//                {
//                    using (IDataReader reader = sqlCommand.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            list.Add((int)reader[0]);
//                        }
//                    }
//                }
//            }

//            return list;
//        }

//        public int? Question_TryGetRandom()
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                connection.Open();
//                string sql = GetSql(SqlEnum.Question_TryGetRandom);

//                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
//                {
//                    using (IDataReader reader = sqlCommand.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            return (int)reader[0];
//                        }
//                    }
//                }
//            }

//            return null;
//        }

//        private string GetSql(SqlEnum sqlEnum)
//        {
//            Assembly assembly = Assembly.GetExecutingAssembly();
//            string resourceName = String.Format("{0}.{1}.sql", sqlEnum.GetType().Namespace, sqlEnum.ToString());
//            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
//            {
//                if (stream == null)
//                {
//                    throw new Exception("Sql not found. Did you forget to make it an embedded resource?");
//                }
//                stream.Position = 0;
//                using (StreamReader reader = new StreamReader(stream))
//                {
//                    return reader.ReadToEnd();
//                }
//            }
//        }
//    }
//}