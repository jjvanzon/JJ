using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using JJ.Framework.Data.SqlClient;
using JJ.Framework.Reflection.Exceptions;
using NHibernate;
using NHibernate.SqlCommand;

namespace JJ.Framework.Data.NHibernate
{
    internal class SqlLogInterceptor : EmptyInterceptor, IInterceptor
    {
        public override SqlString OnPrepareStatement(global::NHibernate.SqlCommand.SqlString sql)
        {
            Debug.WriteLine("");
            Debug.WriteLine(sql.ToString());

            return sql;
        }

        public override void OnExecutingCommand(IDbCommand dbCommand, IDbConnection dbConnection)
        {
            if (dbCommand == null) throw new NullException(() => dbCommand);
            if (dbConnection == null) throw new NullException(() => dbConnection);

            SqlCommand sqlCommand = dbCommand as SqlCommand;
            if (sqlCommand == null)
            {
                throw new IsNotTypeException<SqlCommand>(() => dbCommand);
            }

            string sql = SqlCommandFormatter.Convert(sqlCommand, dbConnection, includeUseStatements: true);
            SqlLogger.WriteLine("");
            SqlLogger.WriteLine(sql);
        }

        public override void AfterTransactionBegin(ITransaction tx)
        {
        }

        public override void AfterTransactionCompletion(ITransaction tx)
        {
        }

        public override void BeforeTransactionCompletion(ITransaction tx)
        {

        }

        public override int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            return base.FindDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override object GetEntity(string entityName, object id)
        {
            return base.GetEntity(entityName, id);
        }

        public override string GetEntityName(object entity)
        {
            return base.GetEntityName(entity);
        }

        public override object Instantiate(string entityName, EntityMode entityMode, object id)
        {
            return base.Instantiate(entityName, entityMode, id);
        }

        public override bool? IsTransient(object entity)
        {
            return base.IsTransient(entity);
        }

        public override void OnCollectionRecreate(object collection, object key)
        {

        }

        public override void OnCollectionRemove(object collection, object key)
        {

        }

        public override void OnCollectionUpdate(object collection, object key)
        {

        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {

        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            return base.OnLoad(entity, id, state, propertyNames, types);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override void PostFlush(System.Collections.ICollection entities)
        {

        }

        public override void PreFlush(System.Collections.ICollection entities)
        {

        }

        public override void SetSession(ISession session)
        {

        }
    }
}