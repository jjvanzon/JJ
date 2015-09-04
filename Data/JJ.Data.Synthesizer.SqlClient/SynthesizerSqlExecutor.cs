﻿using JJ.Framework.Data.SqlClient;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Data.Synthesizer.SqlClient
{
    public class SynthesizerSqlExecutor
    {
        private ISqlExecutor _sqlExecutor;

        public SynthesizerSqlExecutor(ISqlExecutor sqlExecutor)
        {
            if (sqlExecutor == null) throw new NullException(() => sqlExecutor);
            _sqlExecutor = sqlExecutor;
        }

        public IList<int> Document_GetPageOfRootDocumentIDsOrderedByName(int firstIndex, int count)
        {
            return _sqlExecutor.ExecuteReader<int>(SqlEnum.Document_GetPageOfRootDocumentIDsOrderedByName, new { firstIndex, count }).ToArray();
        }

        /// <summary>
        /// Beware that you could get null returned, which either means the database
        /// field is null or that the database record does not exist.
        /// </summary>
        public byte[] Sample_TryGetBytes(int id)
        {
            object obj = _sqlExecutor.ExecuteScalar(SqlEnum.Sample_TryGetBytes, new { id }); ;
            if (obj == DBNull.Value)
            {
                return null;
            }

            byte[] bytes = (byte[])obj;
            return bytes;
        }
        
        /// <summary>
        /// Beware that if the record does not exist in the database,
        /// nothing is updated and you will not get an error message either.
        /// </summary>
        public void Sample_TrySetBytes(int id, byte[] bytes)
        {
            _sqlExecutor.ExecuteNonQuery(SqlEnum.Sample_TrySetBytes, new { id, bytes });
        }

        public int GetID()
        {
            int id = (int)_sqlExecutor.ExecuteScalar(SqlEnum.GetID);
            return id;
        }
    }
}
