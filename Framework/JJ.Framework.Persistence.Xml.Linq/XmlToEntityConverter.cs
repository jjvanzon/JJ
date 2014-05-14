﻿using JJ.Framework.Persistence.Xml.Linq.Internal;
using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace JJ.Framework.Persistence.Xml.Linq
{
    public class XmlToEntityConverter<TEntity>
        where TEntity : class, new()
    {
        private readonly XmlElementAccessor _accessor;

        internal XmlToEntityConverter(XmlElementAccessor accessor)
        {
            if (accessor == null) throw new ArgumentNullException("accessor");
            _accessor = accessor;
        }

        public TEntity ConvertXmlElementToEntity(XElement sourceElement)
        {
            TEntity destEntity = new TEntity();
            ConvertXmlElementToEntity(sourceElement, destEntity);
            return destEntity;
        }

        public void ConvertXmlElementToEntity(XElement sourceElement, TEntity destEntity)
        {
            if (sourceElement == null) throw new ArgumentNullException("sourceElement");
            if (destEntity == null) throw new ArgumentNullException("destEntity");

            foreach (PropertyInfo destProperty in ReflectionCache.GetProperties(typeof(TEntity)))
            {
                string sourceValue = _accessor.GetAttributeValue(sourceElement, destProperty.Name);
                object destValue = ConvertValue(sourceValue, destProperty.PropertyType);
                destProperty.SetValue(destEntity, destValue, null);
            }
        }

        public void ConvertEntityToXmlElement(object sourceEntity, XElement destElement)
        {
            foreach (PropertyInfo sourceProperty in ReflectionCache.GetProperties(typeof(TEntity)))
            {
                // iOS compatibility: PropertyInfo.GetValue in mono on a generic type may cause JIT compilation, which is not supported by iOS.
                //object sourceValue = sourceProperty.GetValue(sourceEntity, null);
                object sourceValue = sourceProperty.GetGetMethod().Invoke(sourceEntity, null);
                string destValue = Convert.ToString(sourceValue);
                _accessor.SetAttributeValue(destElement, sourceProperty.Name, destValue);
            }
        }

        private object ConvertValue(string value, Type type)
        {
            // TODO: Extend with other conversions.
            return System.Convert.ChangeType(value, type);
        }
    }
}
