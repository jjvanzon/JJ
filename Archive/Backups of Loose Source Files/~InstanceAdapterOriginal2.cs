//
//  Circle.Integration.DotNet.InstanceAdapterOriginal2
//
//      Author: Jan-Joost van Zon
//      Date: 2011-03-01 - 2011-03-09
//
//  -----
//
//      Under construction...
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Circle.CodeBase;

namespace Circle.Integration.DotNet
{
    public class InstanceAdapterOriginal2
    {
        public DomainAdapter Domain;
        public TypeAdapter Type;

        public object Object;
        public Item Item;

        // TODO:
        // program it in a way that you use as little .NET reflection information as possible,
        // but base most code on the already derived Circle data.

        public void Convert()
        {
            if (Type == null) return; // Possible for simple types
            
            // Try to construct
            if (Object == null) // Allow manual supply of an instance
            {
                ConstructorInfo parameterlessConstructor = Type.Type.GetConstructor(new Type[] { });
                if (parameterlessConstructor != null)
                {
                    Object = parameterlessConstructor.Invoke(new object[] { });
                }
                else
                {
                    ConstructorInfo typeInitializer = Type.Type.TypeInitializer;
                    if (typeInitializer != null)
                    {
                        Object = typeInitializer.Invoke(new object[] { });
                    }
                }
            }

            // Get out of here if no parameterless constructor or type initializer went off
            if (Object == null) return;

            // Create Item
            Item = Type.Item.CreateInstance();

            // Traverse properties
            foreach (TypePropertyAdapter propertyAdapter in Type.Properties)
            {
                Object value = propertyAdapter.Property.GetValue(Object, null);

                if (value != null)
                {
                    RelatedItem relatedItem = Item.RelatedItems.Single(x => x.ClassMember == propertyAdapter.RelatedItem); // TODO: the predicate is performance-costly.

                    if (propertyAdapter.Type.IsValueType || propertyAdapter.Type == typeof(string)) // TODO: replace by something that looks at Circle data, instead of .NET data.
                    {
                        relatedItem.Item = new Item();
                        relatedItem.Item.Value = value.ToString(); 
                    }
                    else
                    {
                        if (propertyAdapter.TypeAdapter == null) continue; // Possible for simple types?
                        if (propertyAdapter.RelatedItem.IsList) continue; // Skip lists for now

                        ObjectAdapter instance = ConvertObjectMember(propertyAdapter.TypeAdapter, value);
                        relatedItem.Item = instance.Item;
                    }
                }
            }

            foreach (TypeFieldAdapter fieldAdapter in Type.Fields)
            {
                Object value = fieldAdapter.Field.GetValue(Object);

                if (value != null)
                {
                    RelatedItem relatedItem = Item.RelatedItems.Single(x => x.ClassMember == fieldAdapter.RelatedItem); // TODO: the predicate is performance-costly.

                    if (fieldAdapter.Type.IsValueType || fieldAdapter.Type == typeof(string)) // TODO: replace by something that looks at Circle data, instead of .NET data.
                    {
                        relatedItem.Item = new Item();
                        relatedItem.Item.Value = value.ToString(); 
                    }
                    else
                    {
                        if (fieldAdapter.TypeAdapter == null) continue; // Possible for simple types?
                        if (fieldAdapter.RelatedItem.IsList) continue; // Skip lists for now

                        ObjectAdapter instance = ConvertObjectMember(fieldAdapter.TypeAdapter, value);
                        relatedItem.Item = instance.Item;
                    }
                }
            }

            foreach (TypeMethodAdapter methodAdapter in Type.Methods)
            {
                RelatedItem relatedItem = Item.RelatedItems.Single(x => x.ClassMember == methodAdapter.RelatedItem);
                
                relatedItem.Item = methodAdapter.Item.CreateInstance();
            }

            if (Type.Base != null)
            {
                RelatedItem baseRelatedItem = Item.RelatedItems.Single(x => x.ClassMember == Type.Base.RelatedItem);
                //baseRelatedItem.IsBase = true;
                baseRelatedItem.Item = Type.Base.TypeAdapter.Item.CreateInstance();
            }
        }

        private ObjectAdapter ConvertObjectMember(TypeAdapter type, Object value)
        {
            // Declare variables
            Dictionary<Object, ObjectAdapter> typeEntry = null;
            ObjectAdapter instance = null;

            // Lookup existing entry
            if (Domain.Instances.ContainsKey(type.Type))
            {
                typeEntry = Domain.Instances[type.Type];
                if (typeEntry.ContainsKey(value))
                {
                    instance = typeEntry[value];
                }
            }

            // Create type entry
            if (typeEntry == null)
            {
                typeEntry = new Dictionary<Object, ObjectAdapter>();
                Domain.Instances[type.Type] = typeEntry;
            }

            // Create instance entry and convert
            if (instance == null)
            {
                instance = new ObjectAdapter()
                { 
                    Domain = Domain,
                    Type = type, 
                    Object = value 
                };
                typeEntry[value] = instance;
                instance.Convert();
            }

            return instance;
        }
    }
}
