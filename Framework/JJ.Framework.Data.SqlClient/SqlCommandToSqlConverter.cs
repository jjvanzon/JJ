using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Framework.Data.SqlClient
{
    public static class SqlCommandToSqlConverter
    {
        public static string Convert(SqlCommand sqlCommand, bool includeUseStatements)
        {
            if (sqlCommand == null) throw new NullException(() => sqlCommand);

            if (includeUseStatements && String.IsNullOrEmpty(sqlCommand.Connection?.Database))
            {
                throw new Exception("If includeUseStatements is true, then sqlCommand.Connection.Database cannot be null or empty.");
            }

            var sb = new StringBuilder();
            sb.AppendLine(String.Format("use [{0}]", sqlCommand.Connection.Database));

            // TODO: Format its parameters.
            sb.AppendLine(sqlCommand.CommandText);

            return sb.ToString();
        }
    }
}
