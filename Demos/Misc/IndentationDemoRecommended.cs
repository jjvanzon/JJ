using JJ.Framework.Reflection;
using System;
// ReSharper disable UnusedType.Global
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable InvertIf

namespace Demos.Misc
{
    class IndentationDemoRecommended
    {
        object ParseValue(string input, Type type)
        {
            if (type.IsNullableType())
            {
                if (string.IsNullOrEmpty(input))
                {
                    return null;
                }

                type = type.GetUnderlyingNullableTypeFast();
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, input);
            }

            if (type == typeof(TimeSpan))
            {
                return TimeSpan.Parse(input);
            }

            if (type == typeof(Guid))
            {
                return new Guid(input);
            }

            return Convert.ChangeType(input, type);
        }
    }
}
