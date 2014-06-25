﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Xml.Serialization;
using JJ.Framework.Common;
using JJ.Framework.Reflection;
using JJ.Framework.Net45;

namespace JJ.Framework.Xml.Linq.Internal
{
    internal static class ConversionHelper
    {
        private static CultureInfo _culture = new CultureInfo("en-US");

        public static object ParseValue(string input, Type type)
        {
            if (type.IsNullableType())
            {
                if (String.IsNullOrEmpty(input))
                {
                    return null;
                }

                type = type.GetUnderlyingNullableType();
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

            if (type == typeof(IntPtr))
            {
                int number = Int32.Parse(input);
                return new IntPtr(number);
            }

            if (type == typeof(UIntPtr))
            {
                uint number = UInt32.Parse(input);
                return new UIntPtr(number);
            }

            return Convert.ChangeType(input, type, _culture);
        }

        internal static string FormatValue(object value)
        {
            // TODO: Check if ToString is enough to support all the types above.
            // I fear for the nullable types.
            return Convert.ToString(value, _culture);
        }

        /// <summary>
        /// Examines the type and attributes of property 
        /// to determine what type of XML node is expected for it 
        /// (element, attribute or array).
        /// Also verifies that a property is not marked with conflicting attributes.
        /// 
        /// By default a property maps to an element.
        /// You can optionally mark it with the XmlElement attribute to make that extra clear.
        /// 
        /// To map to an XML attribute, mark the property with the XmlAttribute attribute.
        /// 
        /// To map to an array, the property must be of an Array type,
        /// and the XML needs both a parent element that represents the array,
        /// and child elements that represent the array items.
        /// 
        /// If a property is an array type, it cannot be marked with the XmlAttribute or XmlElement attributes.
        /// </summary>
        public static NodeTypeEnum DetermineNodeType(PropertyInfo property)
        {
            // TODO: isCollectionType is always called, even if it is a simple int.
            // Actually, much is called even when it might not be needed and the only reason it is all called,
            // is to check for conflicting annotations, but it might harm performance considerably.

            bool hasXmlAttributeAttribute = property.GetCustomAttribute<XmlAttributeAttribute>() != null;
            bool hasXmlElementAttribute = property.GetCustomAttribute<XmlElementAttribute>() != null;
            bool hasXmlArrayAttribute = property.GetCustomAttribute<XmlArrayAttribute>() != null;
            bool hasXmlArrayItemAttribute = property.GetCustomAttribute<XmlArrayItemAttribute>() != null;
            bool isCollectionType = IsSupportedCollectionType(property.PropertyType);

            if (isCollectionType)
            {
                bool isValid = !hasXmlAttributeAttribute && !hasXmlElementAttribute;
                if (!isValid)
                {
                    throw new Exception(String.Format("Property '{0}' is an Array or is List<T>-assignable and therefore cannot be marked with XmlAttribute or XmlElement. Use XmlArray and XmlArrayItem instead.", property.Name));
                }
                return NodeTypeEnum.Array;
            }

            if (hasXmlAttributeAttribute)
            {
                bool isValid = !hasXmlElementAttribute && !hasXmlArrayAttribute && !hasXmlArrayItemAttribute;
                if (!isValid)
                {
                    throw new Exception(String.Format("Property '{0}' is an XML attribute and therefore cannot be marked with XmlElement, XmlArray or XmlArrayItem.", property.Name));
                }
                return NodeTypeEnum.Attribute;
            }

            // If it is not an array or attribute, then it is an element by default.
            bool isValidElement = !hasXmlAttributeAttribute && !hasXmlArrayAttribute && !hasXmlArrayItemAttribute;
            if (!isValidElement)
            {
                throw new Exception(String.Format("Property '{0}' is an XML element and therefore cannot be marked with XmlAttribute, XmlArray or XmlArrayItem.", property.Name));
            }
            return NodeTypeEnum.Element;
        }

        /// <summary>
        /// Returns whether the type should be handled as an XML Array.
        /// This means whether it is Array or List&lt;T&gt;-assignable.
        /// </summary>
        public static bool IsSupportedCollectionType(Type type)
        {
            bool isArray = type.IsAssignableTo(typeof(Array));
            if (isArray)
            {
                return true;
            }

            bool isSupportedGenericCollection = IsSupportedGenericCollectionType(type);
            if (isSupportedGenericCollection)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns wheter a generic collection type is supported.
        /// The supported types are List&lt;T&gt;, IList&lt;T&gt;, ICollection&lt;T&gt; and IEnumerable&lt;T&gt;.
        /// </summary>
        public static bool IsSupportedGenericCollectionType(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            Type openGenericType = type.GetGenericTypeDefinition();
            if (openGenericType == typeof(List<>)) return true;
            if (openGenericType == typeof(IList<>)) return true;
            if (openGenericType == typeof(IEnumerable<>)) return true;
            if (openGenericType == typeof(ICollection<>)) return true;
            return false;
        }

        /// <summary>
        /// Determines whether a type is considered a single value without any child data members. 
        /// This includes the primitive types (Boolean, Char, Byte, the numeric types and their signed and unsigned variations),
        /// and other types such as String, Guid, DateTime, TimeSpan and Enum types.
        /// </summary>
        public static bool IsLeafType(Type type)
        {
            if (type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(string) ||
                type == typeof(Guid) ||
                type == typeof(DateTime) ||
                type == typeof(TimeSpan))
            {
                return true;
            }

            if (type.IsNullableType())
            {
                Type underlyingType = type.GetUnderlyingNullableType();
                return IsLeafType(underlyingType);
            }

            return false;
        }

        // Names

        public static string FormatCasing(string value, XmlCasingEnum casing)
        {
            switch (casing)
            {
                case XmlCasingEnum.UnmodifiedCase:
                    return value;

                case XmlCasingEnum.CamelCase:
                    return value.StartWithLowerCase();

                default:
                    throw new ValueNotSupportedException(casing);
            }
        }

        /// <summary>
        /// Gets the XML element name for a property.
        /// By default this is the property name converted to camel case 
        /// e.g. MyProperty -&gt; myProperty.
        /// You can also specify the expected XML element name explicity
        /// by marking the property with the XmlElement attribute and specifying the
        /// name with it e.g. [XmlElement("myElement")].
        /// </summary>
        public static string GetElementNameForProperty(PropertyInfo property, XmlCasingEnum casing)
        {
            // Try get element name from XmlElement attribute.
            string name = TryGetXmlElementNameFromAttribute(property);
            if (!String.IsNullOrEmpty(name))
            {
                return name;
            }

            // Otherwise the property name converted to the expected casing (e.g. camel-case).
            name = ConversionHelper.FormatCasing(property.Name, casing);
            return name;
        }

        /// <summary>
        /// Tries to get an XML element name from the XmlElement attribute that the property is marked with,
        /// e.g. [XmlElement("myElement")]. If no name is specified there, returns null or empty string.
        /// </summary>
        private static string TryGetXmlElementNameFromAttribute(PropertyInfo property)
        {
            XmlElementAttribute xmlElementAttribute = property.GetCustomAttribute<XmlElementAttribute>();
            if (xmlElementAttribute != null)
            {
                return xmlElementAttribute.ElementName;
            }

            return null;
        }

        /// <summary>
        /// Gets the XML attribute name for a property.
        /// By default this is the property name converted to camel case 
        /// e.g. MyProperty -&gt; myProperty.
        /// You can also specify the expected XML element name explicity,
        /// by marking the property with the XmlAttribute attribute and specifying the
        /// name with it it e.g. [XmlAttribute("myAttribute")].
        /// </summary>
        public static string GetAttributeNameForProperty(PropertyInfo property, XmlCasingEnum casing)
        {
            // Try get attribute name from XmlAttribute attribute.
            string name = TryGetAttributeNameFromAttribute(property);
            if (!String.IsNullOrEmpty(name))
            {
                return name;
            }

            // Otherwise the property name converted to the expected casing (e.g. camel-case).
            name = ConversionHelper.FormatCasing(property.Name, casing);
            return name;
        }

        /// <summary>
        /// Get the XML attribute name from the XmlAttribute attribute that the property is marked with,
        /// e.g. [XmlAttribute("myAttribute")]. If no name is specified there, returns null or empty string.
        /// </summary>
        private static string TryGetAttributeNameFromAttribute(PropertyInfo property)
        {
            XmlAttributeAttribute xmlAttributeAttribute = property.GetCustomAttribute<XmlAttributeAttribute>();
            if (xmlAttributeAttribute != null)
            {
                return xmlAttributeAttribute.AttributeName;
            }

            return null;
        }

        /// <summary>
        /// Gets the Array XML element name for a collection property.
        /// By default this is the property name converted to camel case 
        /// e.g. MyCollection -&gt; myCollection.
        /// You can also specify the expected XML element name explicity,
        /// by marking the property with the XmlArray attribute and specifying the
        /// name with it it e.g. [XmlArray("myCollection")].
        /// </summary>
        public static string GetXmlArrayNameForCollectionProperty(PropertyInfo collectionProperty, XmlCasingEnum casing)
        {
            // Try get element name from XmlArray attribute.
            string name = TryGetXmlArrayNameFromAttribute(collectionProperty);
            if (!String.IsNullOrEmpty(name))
            {
                return name;
            }

            // Otherwise the property name converted to the expected casing (e.g. camel-case).
            name = ConversionHelper.FormatCasing(collectionProperty.Name, casing);
            return name;
        }

        /// <summary>
        /// Gets an Array XML element name from the XmlArray attribute that the property is marked with,
        /// e.g. [XmlArray("myArray")]. If no name is specified, null or empty string is returned.
        /// </summary>
        private static string TryGetXmlArrayNameFromAttribute(PropertyInfo collectionProperty)
        {
            XmlArrayAttribute xmlArrayAttribute = collectionProperty.GetCustomAttribute<XmlArrayAttribute>();
            if (xmlArrayAttribute != null)
            {
                return xmlArrayAttribute.ElementName;
            }

            return null;
        }

        /// <summary>
        /// Gets the XML element name for an array item for the given collection property.
        /// The XML array item name should always be specified in the XmlArrayItem attribute that the property is marked with.
        /// </summary>
        public static string GetXmlArrayItemNameForCollectionProperty(PropertyInfo collectionProperty)
        {
            // The XML array item name should always be specified in the XmlArrayItem attribute that the property is marked with.
            return GetXmlArrayItemNameFromAttribute(collectionProperty);
        }

        /// <summary>
        /// Gets an XML element name from the XmlArrayItem attribute that the property is marked with.
        /// e.g. [XmlArrayItem("myItem")]. If no name is specified there, an exception is thrown.
        /// </summary>
        private static string GetXmlArrayItemNameFromAttribute(PropertyInfo collectionProperty)
        {
            XmlArrayItemAttribute xmlArrayItemAttribute = collectionProperty.GetCustomAttribute<XmlArrayItemAttribute>();
            if (xmlArrayItemAttribute != null)
            {
                if (!String.IsNullOrEmpty(xmlArrayItemAttribute.ElementName))
                {
                    return xmlArrayItemAttribute.ElementName;
                }
            }

            throw new Exception(String.Format(
                @"Property '{0}' is a collection type, but does specify the XML array item name. " +
                @"Mark the property with an XmlArrayItem attribute, e.g. XmlArrayItem(""myItem"").", collectionProperty.Name));
        }
    }
}
