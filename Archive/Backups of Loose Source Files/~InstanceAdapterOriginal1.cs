//
//  Circle.Integration.DotNet.InstanceAdapterOriginal1
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
    public class InstanceAdapterOriginal1
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

            // Traverse properties
            foreach (PropertyAdapter member in Type.Properties)
            {
                Object value = member.Property.GetValue(Object, null);

                if (value != null)
                {
                    if (member.Type.IsValueType || member.Type == typeof(string)) // TODO: replace by something that looks at Circle data, instead of .NET data.
                    {
                        member.RelatedItem.Item.Value = value.ToString();
                    }
                    else
                    {
                        InstanceAdapter instance = AddMemberInstance(member.TypeAdapter, value);
                        // TODO: assign the created object to the related item of the parent.
                        // member.RelatedItem.Item = member.TypeAdapter.Item.Clone();
                        // PROBLEM:
                        // InstanceAdapter is currently filling the defaults of a type,
                        // not assigning values to a specific instance's member.
                    }
                }
            }

            foreach (FieldAdapter member in Type.Fields)
            {
                Object value = member.Field.GetValue(Object);

                if (value != null)
                {
                    if (member.Type.IsValueType || member.Type == typeof(string)) // TODO: replace by something that looks at Circle data, instead of .NET data.
                    {
                        member.RelatedItem.Item.Value = value.ToString();
                    }
                    else
                    {
                        AddMemberInstance(member.TypeAdapter, value);
                        // TODO: assign the created object to the related item of the parent.
                    }
                }
            }
        }

        private InstanceAdapter AddMemberInstance(TypeAdapter memberType, Object value)
        {
            // Declare variables
            Dictionary<Object, InstanceAdapter> typeEntry = null;
            InstanceAdapter instance = null;

            // Lookup existing entry
            if (Domain.Instances.ContainsKey(Type.Type))
            {
                typeEntry = Domain.Instances[Type.Type];
                if (typeEntry.ContainsKey(Object))
                {
                    instance = typeEntry[Object];
                }
            }

            // Create type entry
            if (typeEntry == null)
            {
                typeEntry = new Dictionary<Object, InstanceAdapter>();
                Domain.Instances[Type.Type] = typeEntry;
            }

            // Create instance entry and convert
            if (instance == null)
            {
                instance = new InstanceAdapter()
                { 
                    Domain = Domain,
                    Type = memberType, 
                    Object = value 
                };
                instance.Convert();
                typeEntry[Object] = instance;
            }

            return instance;
        }

        /* public void Convert_Old()
        {
            if (Type == null) return; // Possible for simple types

            Object obj = null;
            
            // Try to construct
            ConstructorInfo parameterlessConstructor = Type.Type.GetConstructor(new Type[] { });
            if (parameterlessConstructor != null)
            {
                obj = parameterlessConstructor.Invoke(new object[] { });
            }
            else
            {
                ConstructorInfo typeInitializer = Type.Type.TypeInitializer;
                if (typeInitializer != null)
                {
                    obj = typeInitializer.Invoke(new object[] { });
                }
            }

            // Get out of here if no parameterless constructor or type initializer
            if (obj == null) return;

            foreach (PropertyAdapter propertyAdapter in Type.Properties)
            {
                Object value = propertyAdapter.Property.GetValue(obj, null);

                if (propertyAdapter.Type.IsValueType || propertyAdapter.Type == typeof(string)) // TODO: replace by something that looks at Circle data, instead of .NET data.
                {
                    propertyAdapter.RelatedItem.Item.Value = value.ToString();
                }

                if (propertyAdapter.Type.IsClass)
                {
                    InstanceAdapter instance = new InstanceAdapter() { Type = propertyAdapter.TypeAdapter };
                    instance.Convert();
                }
            }

            foreach (FieldAdapter fieldAdapter in Type.Fields)
            {
                Object value = fieldAdapter.Field.GetValue(obj);

                if (fieldAdapter.Type.IsValueType || fieldAdapter.Type == typeof(string)) // TODO: replace by something that looks at Circle data, instead of .NET data.
                {

                    fieldAdapter.RelatedItem.Item.Value = value.ToString();
                }

                if (fieldAdapter.Type.IsClass)
                {
                    InstanceAdapter instance = new InstanceAdapter() { Type = fieldAdapter.TypeAdapter };
                    instance.Convert();
                }
            }
        } */
    }
}
