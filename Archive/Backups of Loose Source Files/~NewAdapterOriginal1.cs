//
//  Circle.Integration.DotNet.NewAdapterOriginal1
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
    public class NewAdapterOriginal1
    {
        public int LayerCount = 9;
        public bool ExcludeExternalTypes = true;
        public bool IgnoreBackingFields = true;

        public bool AddSpaces = true;

        public Item MainItem { get; private set; }

        private readonly DomainAdapter Domain = new DomainAdapter();

        private int MemberCount = 0;
        private int MaxMemberCount = 10000000;

        public void ExecuteWithLiteral(string dotNetLiteral, Item item, bool clear = true, Item parentItem = null)
        {
            if (clear)
            {
                MainItem = item;
                Domain.Assemblies.Clear();
            }

            if (String.IsNullOrEmpty(dotNetLiteral))
            {
                // Default
                ExecuteWithLiteral("Renderer, Circle.Diagram.Engine", item, clear: false); // Assembly.GetExecutingAssembly()
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

            else if (dotNetLiteral.EndsWith(":dotnet"))
            {
                // File path of assembly
                try
                {
                    AssemblyAdapter assembly = Domain.Assemblies.LoadByFilePath(dotNetLiteral.CutRight(":dotnet"));
                    if (assembly != null)
                    {
                        AddAssembly(assembly.Assembly, item, clear: false);
                        if (parentItem != null)
                        {
                            parentItem.RelatedItems.Add(new RelatedItem() { Item = item, Name = item.Name });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: replace error hiding with UI message
                }
            }

            else if (dotNetLiteral.Contains(','))
            {
                // Class of Assembly
                try
                {
                    string[] split = dotNetLiteral.Split(',');
                    string typeName = string.Empty;
                    string namespace_ = string.Empty;
                    string assemblyName = string.Empty;
                    if (split.Length == 2)
                    {
                        typeName = split[0].Trim();
                        assemblyName = split[1].Trim();

                        if (!typeName.Contains('.')) typeName = assemblyName + "." + typeName;
                    }
                    else if (split.Length == 3)
                    {
                        typeName = split[0].Trim();
                        namespace_ = split[1].Trim();
                        assemblyName = split[2].Trim();

                        if (!typeName.Contains('.')) typeName = namespace_ + "." + typeName;
                    }

                    AssemblyAdapter assembly = Domain.Assemblies.LoadByName(assemblyName);
                    if (assembly != null)
                    {
                        item.Name = assembly.Assembly.GetType(typeName, true).Name;
                        if (parentItem != null)
                        {
                            AddChildOfType(parentItem, assembly.Assembly.GetType(typeName, true), typeName);
                        }
                        else
                        {
                            AddMembers(assembly.Assembly.GetType(typeName), item);
                        }
                    }
                    else
                    {
                        // TODO: throw error instead of error hiding.
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
                        Domain.Assemblies.Add(type.Assembly);
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
                        AssemblyAdapter assembly = Domain.Assemblies.LoadByName(dotNetLiteral);
                        if (assembly != null)
                        {
                            AddAssembly(assembly.Assembly, item, clear: false);
                            if (parentItem != null)
                            {
                                parentItem.RelatedItems.Add(new RelatedItem() { Item = item, Name = item.Name });
                            }
                        }
                        else
                        {
                            // TODO: throw exception.
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: replace error hiding with UI message
                }
            }
            if (clear) Domain.Types.Clear();
        }

        public void AddAssembly(Assembly assembly, Item item, bool clear = true)
        { 
            if (assembly == null) return;
            item.Name = assembly.ManifestModule.Name.CutRight(".exe").CutRight(".dll");
            item.IsModule = true;
            MemberCount++;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsNested) continue;
                RelatedItem typeRelatedItem = AddChildOfType(item, type, GetTypeName(type), isPublic: type.IsPublic);
            }
            
            if (clear) Domain.Types.Clear();
        }

        private RelatedItem AddChildOfType(Item parentItem, Type childType, string name, int layer = 1, bool isPublic = false)
        {
            name = name ?? "";

            // Ignore Backing Fields
            if (IgnoreBackingFields && IsBackingField(parentItem, name)) return null;

            // Is List
            bool isList = IsList(ref childType);

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
            else if (Domain.Types.Contains(childType))
            {
                // Existing types
                childItem = Domain.Types.Item(childType).Item;
                addMembers = false;
            }
            else
            {
                // New types
                childItem = new Item() { Name = GetTypeName(childType) };
                //childItem.IsList = isList;
                Domain.Types.Add(childType, childItem);
                addMembers = true;
            }
            RelatedItem childRelatedItem = new RelatedItem() { Item = childItem, Name = name };

            childRelatedItem.IsList = isList;
            childRelatedItem.IsPublic = isPublic;

            parentItem.RelatedItems.Add(childRelatedItem);
            MemberCount++;
            if (addMembers)
            {
                if (layer < LayerCount)
                {
                    if (childType.IsPrimitive || childType == typeof(string)) return childRelatedItem;
                    AddMembers(childType, childRelatedItem.Item, layer);
                }
            }
            return childRelatedItem;
        }

        public void AddMembers(Type type, Item item, int layer = 1)
        {
            if (type == null) return; // What is this for

            // Limit member count
            if (MemberCount >= MaxMemberCount) return;

            // Exclude exteral types
            if (ExcludeExternalTypes && !Domain.Assemblies.Contains(type.Assembly)) return;

            // Add all the members
            AddProperties(type, item, layer);
            AddFields(type, item, layer);
            AddNormalMethods(type, item, layer);
            AddAnonymousMethods(type, item, layer);
            AddInterfaces(type, item, layer);
            AddBaseType(type, item, layer);
            AddNestedTypes(type, item);
        }

        private void AddFields(Type type, Item item, int layer)
        {
            foreach (FieldInfo field in type.GetFields(
                BindingFlags.DeclaredOnly |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic))
            {
                AddField(item, layer, field);
            }
        }

        private void AddField(Item item, int layer, FieldInfo field)
        {
            RelatedItem fieldRelatedItem = AddChildOfType(item, field.FieldType, field.Name, layer + 1, field.IsPublic);
        }

        private void AddProperties(Type type, Item item, int layer)
        {
            foreach (PropertyInfo property in type.GetProperties(
                BindingFlags.DeclaredOnly |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic))
            {
                if (property.DeclaringType != type) continue; // Exclude members of base classes
                AddProperty(property, item, layer);
            }
        }

        private void AddProperty(PropertyInfo property, Item item, int layer)
        {
            RelatedItem propertyRelatedItem = AddChildOfType(item, property.PropertyType, property.Name, layer + 1, isPublic: property.CanRead || property.CanWrite);
        }

        private void AddNormalMethods(Type type, Item item, int layer)
        {
            var normalMethods =
                from method in type.GetMethods(
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Static |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic)
                where !IsAnonymousMethodName(method.Name)
                select method;

            foreach (MethodInfo method in normalMethods)
            {
                if (method.DeclaringType != type) continue; // Exclude members of base classes
                if (method.IsSpecialName) continue; // Exclude methods such as get_ and set_

                AddNormalMethod(method, item, layer);
            }
        }

        private void AddNormalMethod(MethodInfo method, Item parentItem, int layer)
        {
            RelatedItem methodRelatedItem = AddChildOfType(parentItem, null, method.Name, layer + 1, method.IsPublic);
            if (methodRelatedItem == null) return;

            methodRelatedItem.Item.IsCommand = true;

            AddParameters(method, methodRelatedItem.Item, layer);
        }

        private void AddAnonymousMethods(Type type, Item item, int layer)
        {
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
                AddAnonymousMethod(anonymousMethod, item, layer);
            }
        }

        private void AddAnonymousMethod(MethodInfo anonymousMethod, Item item, int layer)
        {
            string parentMethodName = GetParentMethodName(anonymousMethod.Name);
            RelatedItem parentRelatedItem = item.RelatedItems.Find(x => x.Name == parentMethodName);
            if (parentRelatedItem != null)
            {
                Item parentMethodItem = parentRelatedItem.Item;
                RelatedItem methodRelatedItem = AddChildOfType(parentMethodItem, null, "", layer + 1);
                methodRelatedItem.Item.IsCommand = true;

                AddParameters(anonymousMethod, methodRelatedItem.Item, layer);
            }
        }

        private int AddParameters(MethodInfo method, Item methodItem, int layer)
        {
            // TODO: The return value also seems to be displayed as a local variable.
            if (layer < LayerCount)
            {
                layer++;

                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    RelatedItem parameterRelatedItem = AddChildOfType(methodItem, parameter.ParameterType, parameter.Name, layer + 1, isPublic: true);
                }

                ParameterInfo returnParameter = method.ReturnParameter;
                if (returnParameter.ParameterType != typeof(void))
                {
                    AddChildOfType(methodItem, returnParameter.ParameterType, "returns " + returnParameter.ParameterType.Name, layer + 1, isPublic: true);
                }

                MethodBody methodBody = method.GetMethodBody();
                if (methodBody != null)
                {
                    foreach (LocalVariableInfo localVariable in methodBody.LocalVariables)
                    {
                        AddChildOfType(methodItem, localVariable.LocalType, localVariable.LocalType.Name, layer + 1, isPublic: false);
                    }
                }

                layer--;
            }
            return layer;
        }

        private void AddInterfaces(Type type, Item item, int layer)
        {
            // TODO: figure out why this skips interfaces like IOleObject... is that interface visible elsewhere?

            Type[] interfaces = type.GetInterfaces();

            // Exclude interfaces declared inside base classes.
            if (type.BaseType != null) 
            {
                interfaces = interfaces.Except(type.BaseType.GetInterfaces()).ToArray<Type>();
            }

            foreach (Type i in interfaces)
            {
                // If the interface has a declaring type, the interface is not declared by this type.
                if (i.DeclaringType != null) continue;
                AddInterface(item, layer, i);
            }
        }

        private void AddInterface(Item item, int layer, Type i)
        {
            RelatedItem interfaceRelatedItem = AddChildOfType(item, i, GetTypeName(i), layer + 1);
            interfaceRelatedItem.IsBase = true;
            interfaceRelatedItem.Item.IsInterface = true;
        }

        private void AddBaseType(Type type, Item item, int layer)
        {
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
        }

        private void AddNestedTypes(Type type, Item item)
        {
            foreach (Type nestedType in type.GetNestedTypes())
            {
                AddNestedType(item, nestedType);
            }
        }

        private void AddNestedType(Item item, Type nestedType)
        {
            Item nestedTypeItem = new Item() { Name = GetTypeName(nestedType) };
            item.RelatedItems.Add(new RelatedItem() { Item = nestedTypeItem, Name = nestedTypeItem.Name });
            MemberCount++;
            AddChildOfType(item, nestedType, GetTypeName(nestedType), layer: 2, isPublic: nestedType.IsPublic);
        }

        // String resolution

        private bool IsAnonymousMethodName(string methodName)
        {
            return
                methodName.Contains("__") &&
                methodName.Contains("<") &&
                methodName.Contains(">") &&
                methodName.IndexOf("<") < methodName.IndexOf(">");
        }
        
        private string GetParentMethodName(string anonymousMethodName)
        {
            return anonymousMethodName.Between("<", ">");
        }

        /// <summary>
        /// A dirty string-based way to determine if something is a backing field 
        /// for a property that is already defined inside the parent.
        /// It recognizes the backing field based on a few naming conventions.
        /// It also totally ignores any correspondence in type for now.
        /// </summary>
        private bool IsBackingField(Item parentItem, string name)
        {
            if (!parentItem.IsCommand)
            {
                if (parentItem.RelatedItems.Find(x => "_" + x.Name.ToUpper() == name.ToUpper()) != null) return true;
                if (parentItem.RelatedItems.Find(x => x.Name == name.StartWithCap()) != null) return true;
                if (name.EndsWith("k__BackingField")) return true;
            }
            return false;
        }

        /// <summary>
        /// Kind of dirty
        /// </summary>
        private bool IsList(ref Type childType)
        {
            if (childType != null)
            {
                if (childType.GetInterface("ICollection") != null)
                {
                    if (childType.IsGenericType)
                    {
                        childType = childType.GetGenericArguments()[0];
                    }
                    return true;
                }
                if (childType.IsGenericType &&
                        (childType.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                         childType.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    childType = childType.GetGenericArguments()[0];
                    return true;
                }
            }
            return false;
        }

        private string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                StringBuilder typeName = new StringBuilder();
                typeName.Append(type.Name.CutRightUntil("`"));
                if (AddSpaces) typeName.Append(" ");
                typeName.Append("<");
                if (AddSpaces) typeName.Append(" ");

                Type[] genericArguments = type.GetGenericArguments();
                foreach (Type t in genericArguments)
                {
                    typeName.Append(GetTypeName(t)); // Recursive call
                    typeName.Append(", ");
                }
                typeName.Length = typeName.Length - 2; // Remove last comma
                if (AddSpaces) typeName.Append(" ");
                typeName.Append(">");

                return typeName.ToString();
            }
            {
                return type.Name;
            }
        }
    }
}


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

/* if (
    childType.Name == "ICollection`1" ||
    childType.Name == "IList`1")
{
    if (childType.IsGenericType)
    {
        childType = childType.GetGenericArguments()[0];
    }
    isList = true;
} */
