//
//  Circle.Integration.DotNet.AdapterOriginal1
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

namespace Circle.Integration.DotNet
{
    class AdapterOriginal1
    {
        public int LayerCount = 5;

        private int MemberCount = 0;
        private int MaxMemberCount = 10000000;

        public void Execute(Assembly assembly, Item item)
        {
            if (assembly == null) return;
            item.Name = assembly.ManifestModule.Name.CutRight(".exe");
            item.Name = assembly.ManifestModule.Name.CutRight(".dll");
            MemberCount++;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsNested) continue;
                Item typeItem = new Item() { Name = type.Name };
                item.RelatedItems.Add(new RelatedItem() { Item = typeItem, Name = typeItem.Name });
                MemberCount++;
                Execute(type, typeItem, layer: 2);
            }
        }

        public void Execute(Type type, Item item, int layer = 1)
        {
            if (type == null) return;
            if (MemberCount >= MaxMemberCount) return;

            foreach(FieldInfo fieldInfo in type.GetFields()) // The argument BindingFlags.DeclaredOnly is supposed to make sure it will not return the members of bases, but it did not work.
            {
                if (!fieldInfo.IsPublic) continue;
                if (fieldInfo.DeclaringType != type) continue; // Exclude members of base classes
                Item fieldItem = new Item() { Name = fieldInfo.Name };
                item.RelatedItems.Add(new RelatedItem() { Item = fieldItem, Name = fieldItem.Name });
                MemberCount++;
                if (layer < LayerCount)
                {
                    if (fieldInfo.FieldType.IsValueType) continue;
                    if (fieldInfo.FieldType == typeof(string)) continue;
                    Execute(fieldInfo.FieldType, fieldItem, layer + 1);
                }
            }

            foreach(PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.DeclaringType != type) continue; // Exclude members of base classes
                Item propertyItem = new Item() { Name = propertyInfo.Name };
                item.RelatedItems.Add(new RelatedItem() { Item = propertyItem, Name = propertyItem.Name });
                MemberCount++;
                if (layer < LayerCount)
                {
                    if (propertyInfo.PropertyType.IsValueType) continue;
                    if (propertyInfo.PropertyType == typeof(string)) continue;
                    Execute(propertyInfo.PropertyType, propertyItem, layer + 1);
                }
            }

            foreach(MethodInfo methodInfo in type.GetMethods())
            {
                if (methodInfo.DeclaringType != type) continue; // Exclude members of base classes
                if (methodInfo.IsSpecialName) continue; // Exclude methods such as get_ and set_
                Item methodItem = new Item() { Name = methodInfo.Name };
                item.RelatedItems.Add(new RelatedItem() { Item = methodItem, Name = methodItem.Name });
                MemberCount++;
                methodItem.IsCommand = true;
                if (layer < LayerCount)
                {
                    layer++;
                    foreach(ParameterInfo parameterInfo in methodInfo.GetParameters())
                    {
                        Item parameterItem = new Item() { Name = parameterInfo.Name };
                        methodItem.RelatedItems.Add(new RelatedItem() { Item = parameterItem, Name = parameterItem.Name });
                        MemberCount++;
                        Execute(parameterInfo.ParameterType, parameterItem, layer + 1);
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
                Item interfaceItem = new Item() { Name = i.Name };
                interfaceItem.IsInterface = true;
                item.RelatedItems.Add(new RelatedItem() { Item = interfaceItem, Name = interfaceItem.Name, IsBase = true });
                MemberCount++;
                Execute(i, interfaceItem, layer + 1);
            }

            Type baseType = type.BaseType;
            if (baseType != null)
            {
                Item baseItem = new Item() { Name = baseType.Name };
                baseItem.IsClass = true;
                item.RelatedItems.Add(new RelatedItem() { Item = baseItem, Name = baseItem.Name, IsBase = true });
                MemberCount++;
                Execute(baseType, baseItem, layer + 1);
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
