//
//  Circle.Integration.DotNet.AdapterOriginal2
//
//      Author: Jan-Joost van Zon
//      Date: 2010-09-02 - 2011-01-22
//
//  -----

using System;
using System.Reflection;
using Circle.CodeBase;
using Circle.StringFunctions;
using System.Linq;
using System.Collections.Generic;

namespace Circle.Integration.DotNet
{
    class AdapterOriginal2
    {
        public int LayerCount = 4;

        private int MemberCount = 0;
        private int MaxMemberCount = 10000000;

        private readonly Dictionary<Type, Item> Dictionary = new Dictionary<Type, Item>();

        public void Execute(Assembly assembly, Item item)
        {
            if (assembly == null) return;
            item.Name = assembly.ManifestModule.Name.CutRight(".exe").CutRight(".dll");
            MemberCount++;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsNested) continue;
                RelatedItem typeRelatedItem = AddChildOfType(item, type, type.Name);
                Execute(type, typeRelatedItem.Item, layer: 2);
            }
            Dictionary.Clear();
        }

        private RelatedItem AddChildOfType(Item parentItem, Type childType, string name)
        {
            Item childItem;
            if (childType == null)
            {
                childItem = new Item() { Name = name };
            }
            else if (Dictionary.ContainsKey(childType))
            {
                childItem = Dictionary[childType];
            }
            else
            {
                childItem = new Item() { Name = childType.Name };
                Dictionary[childType] = childItem;
            }
            RelatedItem childRelatedItem = new RelatedItem() { Item = childItem, Name = name };
            parentItem.RelatedItems.Add(childRelatedItem);
            MemberCount++;
            return childRelatedItem;
        }

        public void Execute(Type type, Item item, int layer = 1)
        {
            if (type == null) return;
            if (MemberCount >= MaxMemberCount) return;

            foreach(FieldInfo fieldInfo in type.GetFields()) // The argument BindingFlags.DeclaredOnly is supposed to make sure it will not return the members of bases, but it did not work.
            {
                if (!fieldInfo.IsPublic) continue;
                if (fieldInfo.DeclaringType != type) continue; // Exclude members of base classes
                RelatedItem fieldRelatedItem = AddChildOfType(item, fieldInfo.FieldType, fieldInfo.Name);
                if (layer < LayerCount)
                {
                    if (fieldInfo.FieldType.IsValueType) continue;
                    if (fieldInfo.FieldType == typeof(string)) continue;
                    Execute(fieldInfo.FieldType, fieldRelatedItem.Item, layer + 1);
                }
            }

            foreach(PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.DeclaringType != type) continue; // Exclude members of base classes
                RelatedItem propertyRelatedItem = AddChildOfType(item, propertyInfo.PropertyType, propertyInfo.Name);
                if (layer < LayerCount)
                {
                    if (propertyInfo.PropertyType.IsValueType) continue;
                    if (propertyInfo.PropertyType == typeof(string)) continue;
                    Execute(propertyInfo.PropertyType, propertyRelatedItem.Item, layer + 1);
                }
            }

            foreach(MethodInfo methodInfo in type.GetMethods())
            {
                if (methodInfo.DeclaringType != type) continue; // Exclude members of base classes
                if (methodInfo.IsSpecialName) continue; // Exclude methods such as get_ and set_
                RelatedItem methodRelatedItem = AddChildOfType(item, null, methodInfo.Name);
                methodRelatedItem.Item.IsCommand = true;
                if (layer < LayerCount)
                {
                    layer++;
                    foreach(ParameterInfo parameterInfo in methodInfo.GetParameters())
                    {
                        RelatedItem parameterRelatedItem = AddChildOfType(methodRelatedItem.Item, parameterInfo.ParameterType, parameterInfo.Name);
                        Execute(parameterInfo.ParameterType, parameterRelatedItem.Item, layer + 1);
                    }
                }
            }

            Type[] interfaces = type.GetInterfaces();
            if (type.BaseType != null) // Exclude interfaces declared inside base classes.
            {
                interfaces = interfaces.Except(type.BaseType.GetInterfaces()).ToArray<Type>();
            }

            foreach(Type i in interfaces)
            {
                // If the interface has a declaring type, the interface is not declared by this type.
                // TODO: figure out why this skips interfaces like IOleObject... is that interface visible elsewhere?
                if (i.DeclaringType != null) continue;
                RelatedItem interfaceRelatedItem = AddChildOfType(item, i, i.Name);
                interfaceRelatedItem.IsBase = true;
                interfaceRelatedItem.Item.IsInterface = true;
                Execute(i, interfaceRelatedItem.Item, layer + 1);
            }

            Type baseType = type.BaseType;
            if (baseType != null)
            {
                RelatedItem baseRelatedItem = AddChildOfType(item, baseType, baseType.Name);
                baseRelatedItem.IsBase = true;
                baseRelatedItem.Item.IsClass = true;
                Execute(baseType, baseRelatedItem.Item, layer + 1);
            }
            
            /*
            foreach (Type nestedType in type.GetNestedTypes())
            {
                Item typeItem = new Item() { Name = nestedType.Name };
                item.RelatedItems.Add(new RelatedItem() { Item = typeItem, Name = typeItem.Name });
                MemberCount++;
                Execute(type, typeItem, layer: 2);
            }
            */
        }
    }
}
