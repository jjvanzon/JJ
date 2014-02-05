﻿using JJ.Framework.Persistence.Xml.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JJ.Framework.Persistence.Xml
{
    public class XmlContext : ContextBase
    {
        public XmlContext(string folderPath, params Assembly[] modelAssemblies)
            : base(folderPath, modelAssemblies)
        { }

        public override TEntity TryGet<TEntity>(object id)
        {
            EntityStore<TEntity> entityStore = GetEntityStore<TEntity>();
            return entityStore.TryGet(id);
        }

        public override TEntity Create<TEntity>()
        {
            EntityStore<TEntity> entityStore = GetEntityStore<TEntity>();
            return entityStore.Create();
        }

        public override void Insert<TEntity>(TEntity entity)
        {
            EntityStore<TEntity> entityStore = GetEntityStore<TEntity>();
            entityStore.Insert(entity);
        }

        public override void Update<TEntity>(TEntity entity)
        {
            EntityStore<TEntity> entityStore = GetEntityStore<TEntity>();
            entityStore.Update(entity);
        }

        public override void Delete<TEntity>(TEntity entity)
        {
            EntityStore<TEntity> entityStore = GetEntityStore<TEntity>();
            entityStore.Delete(entity);
        }

        public override IEnumerable<TEntity> GetAll<TEntity>()
        {
            EntityStore<TEntity> entityStore = GetEntityStore<TEntity>();
            return entityStore.GetAll();
        }

        public override IEnumerable<TEntity> Query<TEntity>()
        {
            throw new NotSupportedException("XmlContext does not support Query<TEntity>().");
        }

        public override void Commit()
        {
            lock (_lock)
            {
                foreach (IEntityStore entityStore in _entityStoreDictionary.Values)
                {
                    entityStore.Commit();
                }
            }
        }

        public override void Dispose()
        {
            // No code required.
        }

        // Helpers

        private object _lock = new object();
        private Dictionary<string, IEntityStore> _entityStoreDictionary = new Dictionary<string, IEntityStore>();
        
        private EntityStore<TEntity> GetEntityStore<TEntity>()
            where TEntity : class, new()
        {
            lock (_lock)
            {
                string entityName = typeof(TEntity).Name;
                
                if (_entityStoreDictionary.ContainsKey(entityName))
                {
                    return (EntityStore<TEntity>)_entityStoreDictionary[entityName];
                }

                string filePath = Path.Combine(Location, entityName) + ".xml";
                IXmlMapping xmlMapping = XmlMappingResolver.GetXmlMapping(typeof(TEntity), ModelAssemblies);
                EntityStore<TEntity> entityStore = new EntityStore<TEntity>(filePath, xmlMapping);

                _entityStoreDictionary[entityName] = entityStore;

                return entityStore;
            }
        }
    }
}
