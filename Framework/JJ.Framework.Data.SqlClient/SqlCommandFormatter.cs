using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JJ.Framework.Common;
using JJ.Framework.Common.Exceptions;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Framework.Data.SqlClient
{
    public static class SqlCommandFormatter
    {
        private static IFormatProvider _formatProvider = new CultureInfo("en-US");
        
        public static string Convert(SqlCommand sqlCommand)
        {
            return Convert(sqlCommand, null, includeUseStatements: false);
        }

        /// <summary>
        /// Note that an sqlCommand.Connection might be null.
        /// That is why we pass a connection as a separate parameter.
        /// </summary>
        public static string Convert(SqlCommand sqlCommand, IDbConnection dbConnection, bool includeUseStatements)
        {
            if (sqlCommand == null) throw new NullException(() => sqlCommand);
            if (String.IsNullOrWhiteSpace(sqlCommand.CommandText)) throw new NullOrWhiteSpaceException(() => sqlCommand.CommandText);

            if (includeUseStatements)
            {
                if (dbConnection == null)
                {
                    throw new Exception("If includeUseStatements is true, then dbConnection cannot be null.");
                }

                if (String.IsNullOrEmpty(dbConnection.Database))
                {
                    throw new Exception("If includeUseStatements is true, then dbConnection.Database cannot be null or empty.");
                }
            }

            var sb = new StringBuilder();
            sb.Append(String.Format("use [{0}]", dbConnection.Database));
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(sqlCommand.CommandText);

            // Sort parameters in descending order, to prevent clashes when replacing them.
            IList<SqlParameter> sortedSqlParameters = sqlCommand.Parameters.Cast<SqlParameter>()
                                                                           .OrderByDescending(x => x.ParameterName.Length)
                                                                           .ToArray();
            foreach (SqlParameter sqlParameter in sortedSqlParameters)
            {
                string formattedParameterValue = FormatParameter(sqlParameter);

                if (String.IsNullOrEmpty(formattedParameterValue))
                {
                    throw new Exception(
                        "Formatting parameter value failed. Parameter value is null or empty, while it should at least be two quotes (\"''\") or \"null\".");
                }

                if (formattedParameterValue.Contains('@'))
                {
                    throw new Exception("Parameter values with '@' in it are not supported.");
                }

                string formattedParameterName = sqlParameter.ParameterName;
                if (!formattedParameterName.StartsWith("@"))
                {
                    formattedParameterName = "@" + formattedParameterName;
                }

                sb.Replace(formattedParameterName, formattedParameterValue);
            }

            return sb.ToString();
        }

        private static string FormatParameter(SqlParameter sqlParameter)
        {
            if (sqlParameter.Value == null ||
                sqlParameter.Value == DBNull.Value)
            {
                return "null";
            }

            switch (sqlParameter.DbType)
            {
                case DbType.Guid:
                case DbType.String:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                case DbType.Xml: // TODO: Not sure if this should be formatted as a string.
                    {
                        string formattedParameter = FormatSqlStringLiteral(sqlParameter.Value);
                        return formattedParameter;
                    }
                    
                case DbType.Binary:
                    {
                        string formattedParameter = FormatSqlBinaryLiteral(sqlParameter.Value);
                        return formattedParameter;
                    }

                case DbType.Boolean:
                    {
                        string formattedParameter = FormatSqlBoolLiteral(sqlParameter);
                        return formattedParameter;
                    }

                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                    {
                        string formattedParameter = FormatSqlDateLiteral(sqlParameter);
                        return formattedParameter;
                    }

                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.Byte: // TODO: Not sure if this would become a number.
                case DbType.SByte: // TODO: Not sure if this would become a number.
                case DbType.Single:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    {
                        string formattedParameter = FormatSqlNumberLiteral(sqlParameter);
                        return formattedParameter;
                    }

                case DbType.Object:
                case DbType.Time:
                case DbType.DateTimeOffset:
                default:
                    throw new ValueNotSupportedException(sqlParameter.DbType);
            }
        }

        private static string FormatSqlNumberLiteral(SqlParameter sqlParameter)
        {
            string formattedParameterValue = System.Convert.ToString(sqlParameter.Value, _formatProvider);
            return formattedParameterValue;
        }

        private static string FormatSqlDateLiteral(SqlParameter sqlParameter)
        {
            if (!(sqlParameter.Value is DateTime))
            {
                throw new InvalidTypeException<DateTime>(() => sqlParameter.Value);
            }

            DateTime dateTime = (DateTime)sqlParameter.Value;

            string formattedParameterValue = dateTime.ToString("yyyy-MM-dd HH:mi:ss:fff", _formatProvider);
            return formattedParameterValue;
        }

        private static string FormatSqlBoolLiteral(SqlParameter sqlParameter)
        {
            bool castedValue = (bool)sqlParameter.Value;
            if (castedValue == true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        private static string FormatSqlStringLiteral(object value)
        {
            string tempString = System.Convert.ToString(value, _formatProvider);

            if (tempString != null)
            {
                tempString = tempString.Replace("'", "''");
            }

            string formattedString = String.Format("'{0}'", tempString);

            return formattedString;
        }

        private static string FormatSqlBinaryLiteral(object value)
        {
            byte[] bytes = value as byte[];
            if (bytes == null)
            {
                throw new InvalidTypeException<byte[]>(() => value);
            }

            var sb = new StringBuilder();

            sb.Append("0x");

            for (int i = 0; i < bytes.Length; i++)
            {
                uint byteValue = bytes[i];
                string byteString = byteValue.ToString("X"); // "X" stands for hexadecimal.
                byteString = byteString.PadLeft(2, '0');
                
                sb.Append(byteString);
            }

            return sb.ToString();
        }
    }
}
