using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rahl.XmlTransformer
{
    public static class ConvertHelper
    {
        public static object ConvertValue(string strValue, string typeName, string format, NumberStyles? numberStyles = null)
        {
            Type type = null;

            switch (typeName.ToLowerInvariant())
            {
                case "string": type = typeof(string); break;
                case "int": type = typeof(int); break;
                case "long": type = typeof(long); break;
                case "datetime": type = typeof(DateTime); break;
                case "bool": type = typeof(bool); break;
                case "decimal": type = typeof(decimal); break;
                default: throw new Exception("Unrecognized type name: " + typeName);
            }

            return ConvertValue(strValue, type, format, numberStyles);
        }

        public static object ConvertValue(string strValue, Type type, string format, NumberStyles? numberStyles = null)
        {
            if (TryConvertColumnValue(strValue, type, format, out object value, numberStyles))
                return value;

            return GetDefaultValue(type);
        }

        private static bool TryParseDateTimeValue(string input, Type typeOfOutput, string format, out object output)
        {
            DateTime? outValue;
            if (!TryParserForNullableTypes.TryParseNullableDateTime(input, format, out outValue))
            {
                output = null;
                return false;
            }

            if (typeOfOutput == typeof(DateTime))
                output = outValue.GetValueOrDefault();
            else
                output = outValue;

            return true;
        }

        private static bool TryParseBooleanValue(string input, Type typeOfOutput, out object output)
        {
            bool? outValue;
            if (!TryParserForNullableTypes.TryParseNullableBoolean(input, out outValue))
            {
                output = null;
                return false;
            }

            if (typeOfOutput == typeof(bool))
                output = outValue.GetValueOrDefault();
            else
                output = outValue;

            return true;
        }

        private static bool TryParseDecimalValue(string input, Type typeOfOutput, string format, out object output, NumberStyles? numberStyles = null)
        {
            decimal? outValue;

            if (numberStyles == null
                ? !TryParserForNullableTypes.TryParseNullableDecimal(input, out outValue)
                : !TryParserForNullableTypes.TryParseNullableDecimal(input, numberStyles.Value, out outValue))
            {
                output = null;
                return false;
            }

            output = typeOfOutput == typeof(decimal) ? outValue.GetValueOrDefault() : outValue;
            return true;
        }

        private static bool TryParseLongValue(string input, Type typeOfOutput, string format, out object output,
            NumberStyles? numberStyles = null)
        {
            long? outValue;

            if (numberStyles == null
                ? !TryParserForNullableTypes.TryParseNullableLong(input, NumberStyles.Integer | NumberStyles.AllowThousands, out outValue)
                : !TryParserForNullableTypes.TryParseNullableLong(input, numberStyles.Value, out outValue))
            {
                output = null;
                return false;
            }

            output = typeOfOutput == typeof(long) ? outValue.GetValueOrDefault() : outValue;
            return true;
        }

        private static bool TryParseIntValue(string input, Type typeOfOutput, string format, out object output,
            NumberStyles? numberStyles = null)
        {
            int? outValue;

            if (numberStyles == null
                ? !TryParserForNullableTypes.TryParseNullableInt(input, NumberStyles.Integer | NumberStyles.AllowThousands, out outValue)
                : !TryParserForNullableTypes.TryParseNullableInt(input, numberStyles.Value, out outValue))
            {
                output = null;
                return false;
            }

            output = typeOfOutput == typeof(int) ? outValue.GetValueOrDefault() : outValue;
            return true;
        }

        public static bool TryConvertColumnValue(string input, Type targetType, string format, out object output, NumberStyles? numberStyles = null)
        {
            var typeOfOutput = targetType;
            output = GetDefaultValue(targetType);

            if (typeOfOutput == typeof(DateTime) || typeOfOutput == typeof(DateTime?))
            {
                return TryParseDateTimeValue(input, typeOfOutput, format, out output);
            }
            else if (typeOfOutput == typeof(decimal) || typeOfOutput == typeof(decimal?))
            {
                return TryParseDecimalValue(input, typeOfOutput, format, out output, numberStyles);
            }
            else if (typeOfOutput == typeof(long) || typeOfOutput == typeof(long?))
            {
                return TryParseLongValue(input, typeOfOutput, format, out output, numberStyles);
            }
            else if (typeOfOutput == typeof(int) || typeOfOutput == typeof(int?))
            {
                return TryParseIntValue(input, typeOfOutput, format, out output, numberStyles);
            }
            else if (typeOfOutput == typeof(bool) || typeOfOutput == typeof(bool?))
            {
                return TryParseBooleanValue(input, typeOfOutput, out output);
                
            }
            else if (typeOfOutput == typeof(string))
            {
                output = input;
            }
            else
            {
                throw new NotImplementedException("Datatype has not been configured");
            }

            return true;
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
    }
}
