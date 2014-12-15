using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.Sql
{
    internal class SqlExecutor
    {
        private string _connectionString;

        public SqlExecutor(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");

            _connectionString = connectionString;
        }

        public int? TryGetRandomQuestionID()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = GetSql(SqlEnum.TryGetRandomQuestionID);

                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                {
                    using (IDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (int)reader[0];
                        }
                    }
                }
            }

            return null;
        }

        private string GetSql(SqlEnum sqlEnum)
        {
            Assembly assembly =Assembly.GetExecutingAssembly();
            string resourceName = String.Format("{0}.{1}.sql", sqlEnum.GetType().Namespace, sqlEnum.ToString());
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
