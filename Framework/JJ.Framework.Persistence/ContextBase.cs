﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Framework.Persistence
{
    public abstract class ContextBase : IContext
    {
        public ContextBase(string persistenceLocation, params Assembly[] modelAssemblies)
        {
            Location = persistenceLocation;
        }

        public virtual TEntity Get<TEntity>(object id) where TEntity : class, new()
        {
            TEntity entity = TryGet<TEntity>(id);

            if (entity == null)
            {
                throw new Exception(String.Format("Entity of type '{0}' with ID '{1}' not found.", typeof(TEntity).Name, id));
            }

            return entity;
        }

        public abstract TEntity TryGet<TEntity>(object id) where TEntity : class, new();

        public abstract IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, new();

        public abstract TEntity Create<TEntity>() where TEntity : class, new();
        public abstract void Insert<TEntity>(TEntity entity) where TEntity : class;
        public abstract void Update<TEntity>(TEntity entity) where TEntity : class;
        public abstract void Delete<TEntity>(TEntity entity) where TEntity : class;

        public abstract IEnumerable<TEntity> Query<TEntity>() where TEntity : class;

        public abstract void Commit();

        public abstract void Dispose();

        public string Location { get; private set; }
    }
}
