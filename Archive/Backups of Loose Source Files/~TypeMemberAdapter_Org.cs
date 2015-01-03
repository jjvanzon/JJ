//
//  Circle.Integration.DotNet.TypeMemberAdapter
//
//      Author: Jan-Joost van Zon
//      Date: 2011-03-04 - 2011-03-04
//
//  -----

using System;
using Circle.CodeBase;
using System.Collections.Generic;

namespace Circle.Integration.DotNet
{
    /// <summary>
    /// Finds or adds the members' type to the domain,
    /// creates a new related object
    /// and fills in the class, interface and list aspects.
    /// </summary>
    public class TypeMemberAdapter : AdapterWithReference
    {
        public Type Type; // Input

        public TypeAdapter TypeAdapter; // Output

        public override void Convert()
        {
            Reference = new Reference();

            // Alternative for omitting primitives
            if (!Domain.AddPrimitives && Type.IsPrimitive ||
                 Domain.ExcludeAdditionalPrimitives && Domain.AdditionalPrimitives.Contains(Helpers.TypeOrUnderlyingGeneric(Type)) )
            {
                Reference.Name = Helpers.GetFormattedTypeName(Type, Domain.AddSpacesToIdentifiers);
                Reference.Object = new Obj();
                Reference.Object.Name = Type.Name;
                return;
            }

            TypeAdapter = Domain.Types.Get(Type);

            // If type does not exist
            if (TypeAdapter == null)
            {
                TypeAdapter = Domain.Types.Add(Type);
                TypeAdapter.Convert();
                TypeAdapter.IsPlaceholder = true;
            }

            if (Domain.ShowClassRelationsAsObjectRelations)
            {
                // Treat class-relations as object-relations
                Reference.Object = TypeAdapter;
            }
            else
            {
                // Differentiate between class and interface relations
                if (TypeAdapter.Type.IsClass) Reference.Class = TypeAdapter;
                if (TypeAdapter.Type.IsValueType) Reference.Class = TypeAdapter;
                if (TypeAdapter.Type.IsInterface) Reference.Interface = TypeAdapter;
            }

            Reference.IsList = TypeAdapter.IsList;
            // TODO: lists probably need different class-handling too.
        }
    }
}
