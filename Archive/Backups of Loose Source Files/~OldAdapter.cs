//
//  Circle.Integration.DotNet.OldAdapter
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
    public class Adapter
    {
        public int LayerCount = 9;
        public bool ExcludeExternalTypes = true;
        public bool IgnoreBackingFields = true;

        public bool AddSpaces = true;

        public Obj MainObject { get; private set; }

        private readonly DomainAdapter Domain = new DomainAdapter();

        private int MemberCount = 0;
        private int MaxMemberCount = 10000000;

        public void ExecuteWithLiteral(string dotNetLiteral, Obj obj, bool clear = true, Obj parentObject = null)
        {
            if (clear)
            {
                MainObject = obj;
                Domain.Assemblies.Clear();
            }

            if (String.IsNullOrEmpty(dotNetLiteral))
            {
                // Default
                ExecuteWithLiteral("Renderer, Circle.Diagram.Engine", obj, clear: false); // Assembly.GetExecutingAssembly()
                obj.IsModule = true;
            }

            else if (dotNetLiteral.Contains("/"))
            {
                // Combination of literals
                obj.Name = "Combination";
                obj.IsModule = true;
                foreach (string str in dotNetLiteral.Split('/'))
                {
                    string trimmedString = str.Trim();
                    Obj object2 = new Obj();
                    ExecuteWithLiteral(trimmedString, object2, clear: false, parentObject: obj);
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
                        AddAssembly(assembly.Assembly, obj, clear: false);
                        if (parentObject != null)
                        {
                            parentObject.AddReference(new Reference() { Object = obj, Name = obj.Name });
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
                        obj.Name = assembly.Assembly.GetType(typeName, true).Name;
                        if (parentObject != null)
                        {
                            AddChildOfType(parentObject, assembly.Assembly.GetType(typeName, true), typeName);
                        }
                        else
                        {
                            AddMembers(assembly.Assembly.GetType(typeName), obj);
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
                        obj.Name = GetTypeName(type);
                        if (parentObject != null)
                        {
                            AddChildOfType(parentObject, type, obj.Name);
                        }
                        else
                        {
                            AddMembers(type, obj);
                        }
                    }
                    else
                    {
                        // Single Assembly
                        AssemblyAdapter assembly = Domain.Assemblies.LoadByName(dotNetLiteral);
                        if (assembly != null)
                        {
                            AddAssembly(assembly.Assembly, obj, clear: false);
                            if (parentObject != null)
                            {
                                parentObject.AddReference(new Reference() { Object = obj, Name = obj.Name });
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

        public void AddAssembly(Assembly assembly, Obj obj, bool clear = true)
        { 
            if (assembly == null) return;
            obj.Name = assembly.ManifestModule.Name.CutRight(".exe").CutRight(".dll");
            obj.IsModule = true;
            MemberCount++;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsNested) continue;
                AddChildOfType(obj, type, GetTypeName(type), isPublic: type.IsPublic);
            }
            
            if (clear) Domain.Types.Clear();
        }

        private Reference AddChildOfType(Obj parentObject, Type childType, string name, int layer = 1, bool isPublic = false)
        {
            name = name ?? "";

            // Ignore Backing Fields
            if (IgnoreBackingFields && IsBackingField(parentObject, name)) return null;

            // Is List
            bool isList = IsList(ref childType);

            IObj childObject;
            bool addMembers;
            if (childType == null)
            {
                // Methods
                childObject = new Obj() { Name = name };
                addMembers = false;
            }
            else if (childType.IsPrimitive || childType == typeof(string)) // used to be childType.IsValueType
            {
                // Primitive Types
                childObject = new Obj() { Name = name };
                addMembers = false;
            }
            else if (Domain.Types.Contains(childType))
            {
                // Existing types
                childObject = Domain.Types.Get(childType);
                addMembers = false;
            }
            else
            {
                // New types
                childObject = new Obj() { Name = GetTypeName(childType) };
                //childObject.IsList = isList;
                Domain.Types.Add(childType, childObject);
                addMembers = true;
            }
            Reference childReference = new Reference() { Object = childObject, Name = name };

            childReference.IsList = isList;
            childReference.IsPublic = isPublic;

            parentObject.AddReference(childReference);
            MemberCount++;
            if (addMembers)
            {
                if (layer < LayerCount)
                {
                    if (childType.IsPrimitive || childType == typeof(string)) return childReference;
                    AddMembers(childType, (Obj)childReference.Object, layer);
                }
            }
            return childReference;
        }

        public void AddMembers(Type type, Obj obj, int layer = 1)
        {
            if (type == null) return; // What is this for

            // Limit member count
            if (MemberCount >= MaxMemberCount) return;

            // Exclude exteral types
            if (ExcludeExternalTypes && !Domain.Assemblies.Contains(type.Assembly)) return;

            // Add all the members
            AddProperties(type, obj, layer);
            AddFields(type, obj, layer);
            AddNormalMethods(type, obj, layer);
            AddAnonymousMethods(type, obj, layer);
            AddInterfaces(type, obj, layer);
            AddBaseType(type, obj, layer);
            AddNestedTypes(type, obj);
        }

        private void AddProperties(Type type, Obj obj, int layer)
        {
            foreach (PropertyInfo property in type.GetProperties(
                BindingFlags.DeclaredOnly |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic))
            {
                if (property.DeclaringType != type) continue; // Exclude members of base classes
                AddProperty(property, obj, layer);
            }
        }

        private void AddProperty(PropertyInfo property, Obj obj, int layer)
        {
            Reference propertyReference = AddChildOfType(obj, property.PropertyType, property.Name, layer + 1, isPublic: property.CanRead || property.CanWrite);
        }

        private void AddFields(Type type, Obj obj, int layer)
        {
            foreach (FieldInfo field in type.GetFields(
                BindingFlags.DeclaredOnly |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic))
            {
                AddField(obj, layer, field);
            }
        }

        private void AddField(Obj obj, int layer, FieldInfo field)
        {
            Reference fieldReference = AddChildOfType(obj, field.FieldType, field.Name, layer + 1, field.IsPublic);
        }

        private void AddNormalMethods(Type type, Obj obj, int layer)
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

                AddNormalMethod(method, obj, layer);
            }
        }

        private void AddNormalMethod(MethodInfo method, Obj parentObject, int layer)
        {
            Reference methodReference = AddChildOfType(parentObject, null, method.Name, layer + 1, method.IsPublic);
            if (methodReference == null) return;

            methodReference.Object.IsCommand = true;

            AddParameters(method, (Obj)methodReference.Object, layer);
        }

        private void AddAnonymousMethods(Type type, Obj obj, int layer)
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
                AddAnonymousMethod(anonymousMethod, obj, layer);
            }
        }

        private void AddAnonymousMethod(MethodInfo anonymousMethod, Obj obj, int layer)
        {
            string parentMethodName = GetParentMethodName(anonymousMethod.Name);
            Reference parentReference = (Reference)obj.References.Find(x => x.Name == parentMethodName);
            if (parentReference != null)
            {
                Obj parentMethodObject = (Obj)parentReference.Object;
                Reference methodReference = AddChildOfType(parentMethodObject, null, "", layer + 1);
                methodReference.Object.IsCommand = true;

                AddParameters(anonymousMethod, (Obj)methodReference.Object, layer);
            }
        }

        private int AddParameters(MethodInfo method, Obj methodObject, int layer)
        {
            // TODO: The return value also seems to be displayed as a local variable.
            if (layer < LayerCount)
            {
                layer++;

                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    Reference parameterReference = AddChildOfType(methodObject, parameter.ParameterType, parameter.Name, layer + 1, isPublic: true);
                }

                ParameterInfo returnParameter = method.ReturnParameter;
                if (returnParameter.ParameterType != typeof(void))
                {
                    AddChildOfType(methodObject, returnParameter.ParameterType, "returns " + returnParameter.ParameterType.Name, layer + 1, isPublic: true);
                }

                MethodBody methodBody = method.GetMethodBody();
                if (methodBody != null)
                {
                    foreach (LocalVariableInfo localVariable in methodBody.LocalVariables)
                    {
                        AddChildOfType(methodObject, localVariable.LocalType, localVariable.LocalType.Name, layer + 1, isPublic: false);
                    }
                }

                layer--;
            }
            return layer;
        }

        private void AddInterfaces(Type type, Obj obj, int layer)
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
                AddInterface(obj, layer, i);
            }
        }

        private void AddInterface(Obj obj, int layer, Type i)
        {
            Reference interfaceReference = AddChildOfType(obj, i, GetTypeName(i), layer + 1);
            interfaceReference.IsBase = true;
            interfaceReference.Object.IsInterface = true;
        }

        private void AddBaseType(Type type, Obj obj, int layer)
        {
            Type baseType = type.BaseType;
            if (baseType != null)
            {
                if (baseType != typeof(object))
                {
                    Reference baseReference = AddChildOfType(obj, baseType, GetTypeName(baseType), layer + 1);
                    baseReference.IsBase = true;
                    baseReference.Object.IsClass = true;
                }
            }
        }

        private void AddNestedTypes(Type type, Obj obj)
        {
            foreach (Type nestedType in type.GetNestedTypes())
            {
                AddNestedType(obj, nestedType);
            }
        }

        private void AddNestedType(Obj obj, Type nestedType)
        {
            Obj nestedTypeObject = new Obj() { Name = GetTypeName(nestedType) };
            obj.AddReference(new Reference() { Object = nestedTypeObject, Name = nestedTypeObject.Name });
            MemberCount++;
            AddChildOfType(obj, nestedType, GetTypeName(nestedType), layer: 2, isPublic: nestedType.IsPublic);
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
        private bool IsBackingField(Obj parentObject, string name)
        {
            if (!parentObject.IsCommand)
            {
                if (parentObject.References.Find(x => "_" + x.Name.ToUpper() == name.ToUpper()) != null) return true;
                if (parentObject.References.Find(x => x.Name == name.StartWithCap()) != null) return true;
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
            else
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
        AddChildOfType(MainObj, baseMethod.DeclaringType, GetTypeName(baseMethod.DeclaringType));
    }

    // If adding base type worked,
    // Add reference to base method
    if (Dictionary.ContainsKey(baseMethod.DeclaringType))
    {
        Obj baseObj = Dictionary[baseMethod.DeclaringType];
        Reference baseReference = baseObj.References.Find(x => x.Name == method.Name);
        if (baseReference != null) // Some base methods are not shown, e.g. Finalize().
        {
            Obj baseMethodObj = baseReference.Obj; 
            methodReference = new Reference();
            methodReference.Obj = baseMethodObj;
            methodReference.Name = baseMethodObj.Name;
            obj.AddReference(methodReference);
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
