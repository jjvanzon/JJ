//
//  Circle.Integration.DotNet.AdapterOriginal3
//
//      Author: Jan-Joost van Zon
//      Date: 2010-09-02 - 2011-01-22
//
//  -----

using System;
using System.Reflection;
using System.Text;
using Circle.CodeBase;
using Circle.StringFunctions;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Circle.Integration.DotNet
{
    public class AdapterOriginal3
    {
        public int LayerCount = 9;
        public bool ExcludeExternalTypes = true;

        public Item MainItem { get; private set; }
        public readonly List<Assembly> Assemblies = new List<Assembly>();
        private readonly Dictionary<Type, Item> Dictionary = new Dictionary<Type, Item>();

        private int MemberCount = 0;
        private int MaxMemberCount = 10000000;

        public void ExecuteWithLiteral(string dotNetLiteral, Item item, bool clear = true, Item parentItem = null)
        {
            if (clear)
            {
                MainItem = item;
                Assemblies.Clear();
            }

            if (String.IsNullOrEmpty(dotNetLiteral))
            {
                // Default
                ExecuteWithLiteral("Circle.Diagram.Entities", item, clear: false); // Assembly.GetExecutingAssembly()
                item.IsModule = true;
            }

            else if (dotNetLiteral.Contains("/"))
            {
                // Combination of literals
                item.Name = "Combination";
                item.IsModule = true;
                foreach (string str in dotNetLiteral.Split('/'))
                {
                    string trimmedString = str.Trim();
                    Item item2 = new Item();
                    ExecuteWithLiteral(trimmedString, item2, clear: false, parentItem: item);
                }
            }

            else if (dotNetLiteral.Contains(','))
            {
                // Class of Assembly
                try
                {
                    string[] split = dotNetLiteral.Split(',');
                    string assemblyName = split[1].Trim();
                    string typeName = split[0].Trim();
                    if (!typeName.Contains('.')) typeName = assemblyName + "." + typeName;
                    Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
                    Assemblies.Add(assembly);
                    item.Name = typeName;
                    if (parentItem != null)
                    {
                        AddChildOfType(parentItem, assembly.GetType(typeName), typeName);
                    }
                    else
                    {
                        AddMembers(assembly.GetType(typeName), item);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: replace error hiding with UI message
                }
            }

            else
            {
                // Single Assembly or Single Type
                try
                {
                    // Single Type
                    // TODO: this does not work well. You have to try to load the qualifier as an assembly.
                    Type type = Type.GetType(dotNetLiteral);
                    if (type != null)
                    {
                        Assemblies.Add(type.Assembly);
                        item.Name = GetTypeName(type);
                        if (parentItem != null)
                        {
                            AddChildOfType(parentItem, type, item.Name);
                        }
                        else
                        {
                            AddMembers(type, item);
                        }
                    }
                    else
                    {
                        // Single Assembly
                        // TODO: this does not work well. It does not accept e.g. 'System.Drawing'.
                        Assembly assembly = Assembly.LoadWithPartialName(dotNetLiteral);
                        if (assembly != null)
                        {
                            item.IsModule = true;
                            ExecuteForAssembly(assembly, item, clear: false);
                            if (parentItem != null)
                            {
                                parentItem.RelatedItems.Add(new RelatedItem() { Item = item, Name = item.Name });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: replace error hiding with UI message
                }
            }
            if (clear) Dictionary.Clear();
        }

        public void ExecuteForAssembly(Assembly assembly, Item item, bool clear = true)
        {
            if (assembly == null) return;
            Assemblies.Add(assembly);
            item.Name = assembly.ManifestModule.Name.CutRight(".exe").CutRight(".dll");
            MemberCount++;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsNested) continue;
                RelatedItem typeRelatedItem = AddChildOfType(item, type, GetTypeName(type));
            }
            
            if (clear) Dictionary.Clear();
        }

        public System.Collections.ArrayList ArrayList = new ArrayList();

        private RelatedItem AddChildOfType(Item parentItem, Type childType, string name, int layer = 1)
        {
            // Is List
            bool isList = false;
            if (childType != null)
            {
                if (childType.GetInterface("ICollection") != null)
                {
                    if (childType.IsGenericType)
                    {
                        childType = childType.GetGenericArguments()[0];
                    }
                    isList = true;
                }
            }

            Item childItem;
            bool addMembers;
            if (childType == null)
            {
                // Methods
                childItem = new Item() { Name = name };
                addMembers = false;
            }
            else if (childType.IsPrimitive || childType == typeof(string)) // used to be childType.IsValueType
            {
                // Primitive Types
                childItem = new Item() { Name = name };
                addMembers = false;
            }
            else if (Dictionary.ContainsKey(childType))
            {
                // Existing types
                childItem = Dictionary[childType];
                addMembers = false;
            }
            else
            {
                // New types
                childItem = new Item() { Name = GetTypeName(childType) };
                //childItem.IsList = isList;
                Dictionary[childType] = childItem;
                addMembers = true;
            }
            RelatedItem childRelatedItem = new RelatedItem() { Item = childItem, Name = name };

            childRelatedItem.IsList = isList;
            parentItem.RelatedItems.Add(childRelatedItem);
            MemberCount++;
            if (addMembers)
            {
                if (layer < LayerCount)
                {
                    //if (childType.IsValueType || childType == typeof(string)) return childRelatedItem;
                    if (childType.IsPrimitive || childType == typeof(string)) return childRelatedItem;
                    AddMembers(childType, childRelatedItem.Item, layer);
                }
            }
            return childRelatedItem;
        }

        public void AddMembers(Type type, Item item, int layer = 1)
        {
            if (type == null) return;
            if (MemberCount >= MaxMemberCount) return;
            if (ExcludeExternalTypes)
            {
                if (!Assemblies.Contains(type.Assembly))
                {
                    return;
                }
            }

            foreach(FieldInfo fieldInfo in type.GetFields()) // The argument BindingFlags.DeclaredOnly is supposed to make sure it will not return the members of bases, but it did not work.
            {
                if (fieldInfo.DeclaringType != type) continue; // Exclude members of base classes
                RelatedItem fieldRelatedItem = AddChildOfType(item, fieldInfo.FieldType, fieldInfo.Name, layer + 1);
            }

            foreach(PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.DeclaringType != type) continue; // Exclude members of base classes
                RelatedItem propertyRelatedItem = AddChildOfType(item, propertyInfo.PropertyType, propertyInfo.Name, layer + 1);
            }

            foreach(MethodInfo method in type.GetMethods(
                BindingFlags.DeclaredOnly | 
                BindingFlags.Static | 
                BindingFlags.Instance |
                BindingFlags.Public | 
                BindingFlags.NonPublic))
            {
                if (method.DeclaringType != type) continue; // Exclude members of base classes
                if (method.IsSpecialName) continue; // Exclude methods such as get_ and set_
                if (IsAnonymousMethodName(method.Name)) continue; // Exclude anonymous methods (in a dirty way)

                RelatedItem methodRelatedItem;

                // Handle overrides

                // This code is disabled.
                // This does not work well when methods contain anonymous methods as their definition,
                // and object lines are misused as class lines. Then there will be e.g. a single OnExecute method
                // that will get the contents of all the OnExecute overrides put together.
                // This because the base OnExecute is the class's OnExecute, not the specific Object's OnExecute.

                // TODO: make both handling overrides and handling anonymous methods options that you can switch on or off.

                /*
                MethodInfo baseMethod = method.GetBaseDefinition();

                if (!ReferenceEquals(method, baseMethod))
                {
                    // Add base type if it does not exist
                    if 
                    (
                        !Dictionary.ContainsKey(baseMethod.DeclaringType) &&
                        baseMethod.DeclaringType.Assembly == CurrentAssembly &&
                        !baseMethod.DeclaringType.IsNested
                    )
                    {
                        AddChildOfType(MainItem, baseMethod.DeclaringType, GetTypeName(baseMethod.DeclaringType));
                    }

                    // If adding base type worked,
                    // Add reference to base method
                    if (Dictionary.ContainsKey(baseMethod.DeclaringType))
                    {
                        Item baseItem = Dictionary[baseMethod.DeclaringType];
                        RelatedItem baseMethodRelatedItem = baseItem.RelatedItems.Find(x => x.Name == method.Name);
                        if (baseMethodRelatedItem != null) // Some base methods are not shown, e.g. Finalize().
                        {
                            Item baseMethodItem = baseMethodRelatedItem.Item; 
                            methodRelatedItem = new RelatedItem();
                            methodRelatedItem.Item = baseMethodItem;
                            methodRelatedItem.Name = baseMethodItem.Name;
                            item.RelatedItems.Add(methodRelatedItem);
                            continue;
                        }
                    }
                }
                */

                methodRelatedItem = AddChildOfType(item, null, method.Name, layer + 1);
                methodRelatedItem.Item.IsCommand = true;

                if (layer < LayerCount)
                {
                    layer++;
                    // TODO: check if you have to use methodInfo.ReturnParameter too.
                    foreach(ParameterInfo parameterInfo in method.GetParameters())
                    {
                        RelatedItem parameterRelatedItem = AddChildOfType(methodRelatedItem.Item, parameterInfo.ParameterType, parameterInfo.Name, layer + 1);
                    }
                    layer--;
                }
            }

            var anonymousMethods = 
                from method in type.GetMethods(
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Static |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic)
                where IsAnonymousMethodName(method.Name)
                select method;

            foreach (MethodInfo anonymousMethod in anonymousMethods)
            {
                string parentMethodName = anonymousMethod.Name.Between("<", ">");
                RelatedItem parentRelatedItem = item.RelatedItems.Find(x => x.Name == parentMethodName);
                if (parentRelatedItem != null)
                {
                    Item parentItem = parentRelatedItem.Item;
                    RelatedItem methodRelatedItem = AddChildOfType(parentItem, null, "", layer + 1);
                    //RelatedItem methodRelatedItem = AddChildOfType(parentItem, null, methodInfo.Name, layer + 1);
                    methodRelatedItem.Item.IsCommand = true;

                    if (layer < LayerCount)
                    {
                        layer++;
                        // TODO: check if you have to use methodInfo.ReturnParameter too.
                        foreach (ParameterInfo parameterInfo in anonymousMethod.GetParameters())
                        {
                            RelatedItem parameterRelatedItem = AddChildOfType(methodRelatedItem.Item, parameterInfo.ParameterType, parameterInfo.Name, layer + 1);
                        }
                        layer--;
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
                RelatedItem interfaceRelatedItem = AddChildOfType(item, i, GetTypeName(i), layer + 1);
                interfaceRelatedItem.IsBase = true;
                interfaceRelatedItem.Item.IsInterface = true;
            }

            Type baseType = type.BaseType;
            if (baseType != null)
            {
                if (baseType != typeof(object))
                {
                    RelatedItem baseRelatedItem = AddChildOfType(item, baseType, GetTypeName(baseType), layer + 1);
                    baseRelatedItem.IsBase = true;
                    baseRelatedItem.Item.IsClass = true;
                }
            }

            // TODO: use this
            /*
            foreach (Type nestedType in type.GetNestedTypes())
            {
                Item typeItem = new Item() { Name = GetTypeName(nestedType) };
                item.RelatedItems.Add(new RelatedItem() { Item = typeItem, Name = typeItem.Name });
                MemberCount++;
                Execute(type, typeItem, layer: 2);
            }
            */
        }

        private bool IsAnonymousMethodName(string methodName)
        {
            return
                methodName.Contains("__") &&
                methodName.Contains("<") &&
                methodName.Contains(">") &&
                methodName.IndexOf("<") < methodName.IndexOf(">");
        }

        private string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                StringBuilder typeName = new StringBuilder();
                typeName.Append(type.Name.CutRightUntil("`"));
                typeName.Append("<");

                Type[] genericArguments = type.GetGenericArguments();
                foreach (Type t in genericArguments)
                {
                    typeName.Append(GetTypeName(t)); // Recursive call
                    typeName.Append(", ");
                }
                typeName.Length = typeName.Length - 2; // Remove last comma
                typeName.Append(">");

                return typeName.ToString();
            }
            {
                return type.Name;
            }
        }
    }
}
