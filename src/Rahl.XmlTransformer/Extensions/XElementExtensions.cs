using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Rahl.XmlTransformer;

namespace Rahl.XmlTransformer
{
    public static class XElementExtensions
    {
        public static T GetValue<T>(this XElement element, string name, string format = null,
            Func<string, string> coerceRawValue = null, NumberStyles? numberStyles = null)
        {
            string val = element.GetValue(name);

            if (coerceRawValue != null)
            {
                val = coerceRawValue(val);
            }

            return (T)ConvertValue(val, typeof(T), format, numberStyles);
        }

        public static string GetValue(this XElement element, string[] paths)
        {
            XElement child = element;

            foreach (var path in paths)
            {
                child = child?.Element(path);
            }

            return child?.Value;
        }

        public static string GetValue(this XElement element, string name)
        {
            var child = element.Element(name);
            if (child == null)
                return null;

            try
            {
                return child.Value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Field: {name}, Value: {child.Value}, Error: {ex.Message}", ex);
            }
        }

        public static string GetValueOrDefault(this XElement element, string name, string defaultValue)
        {
            var node = element.Element(name);
            if (node == null) return defaultValue;

            try { return node.Value; }
            catch (Exception) { return defaultValue; }
        }

        public static T GetAttributeValue<T>(this XmlReader reader, string name, string format = null)
        {
            string val = reader.GetAttributeValue(name);

            return (T)ConvertValue(val, typeof(T), format);
        }

        public static string GetAttributeValue(this XmlReader reader, string name)
        {
            try
            {
                return reader.GetAttribute(name);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Attribute: {name}, Error: {ex.Message}", ex);
            }
        }

        public static T GetAttributeValue<T>(this XElement element, string name, string format = null)
        {
            string val = element.GetAttributeValue(name);

            return (T)ConvertValue(val, typeof(T), format);
        }

        public static string GetAttributeValue(this XElement element, string name)
        {
            var attr = element.Attribute(name);
            if (attr == null)
                return null;

            try
            {
                return attr.Value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Field: {name}, Value: {attr.Value}, Error: {ex.Message}", ex);
            }
        }

        public static object ConvertValue(string strValue, Type type, string format, NumberStyles? numberStyles = null)
        {
            if (ConvertHelper.TryConvertColumnValue(strValue, type, format, out object value, numberStyles))
                return value;

            return GetDefaultValue(type);
        }

        private static object GetDefaultValue(Type type)
        {
            object value = null;
            if (type.IsValueType)
            {
                value = Activator.CreateInstance(type);
            }
            return value;
        }

        public static IEnumerable<XElement> ElementsNamed(this XElement element, string name)
        {
            return element.Elements().Where(p => p.Name.LocalName == name);
        }

        public static XElement ElementNamed(this XElement element, string name)
        {
            return element.Elements().FirstOrDefault(p => p.Name.LocalName == name);
        }

        public static XAttribute AttributeNamed(this XElement element, string name)
        {
            return element.Attributes().FirstOrDefault(p => p.Name.LocalName == name);
        }
    }
}