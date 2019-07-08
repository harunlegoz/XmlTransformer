using System;
using System.Globalization;

namespace Rahl.XmlTransformer
{
    public class TryParserForNullableTypes
    {
        private delegate bool TryPassSignature<T>(string s, out T result);
        private delegate bool TryPassSignatureNumeric<T>(string s, NumberStyles style, IFormatProvider provider, out T result);

        private static bool TryParseNullable<T>(string s, out T? result, TryPassSignature<T> tryPassMethod) where T : struct
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return true;
            }
            else
            {
                T temp;
                bool success = tryPassMethod(s, out temp);
                result = temp;
                return success;
            }
        }

        private static bool TryParseNullable<T>(string s, NumberStyles styles, out T? result, TryPassSignatureNumeric<T> tryPassMethod) where T : struct
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return true;
            }
            else
            {
                T temp;
                bool success = tryPassMethod(s, styles, NumberFormatInfo.CurrentInfo, out temp);
                result = temp;
                return success;
            }
        }

        public static bool TryParseNullableInt(string s, out int? result)
        {
            return TryParseNullable(s, out result, int.TryParse);
        }

        public static bool TryParseNullableInt(string s, NumberStyles numericStyles, out int? result)
        {
            return TryParseNullable(s, numericStyles, out result, int.TryParse);
        }

        public static bool TryParseNullableDecimal(string s, out decimal? result)
        {
            return TryParseNullable(s, out result, decimal.TryParse);
        }

        public static bool TryParseNullableDecimal(string s, NumberStyles numericStyles, out decimal? result)
        {
            return TryParseNullable(s, numericStyles, out result, decimal.TryParse);
        }

        public static bool TryParseNullableLong(string s, out long? result)
        {
            return TryParseNullable(s, out result, long.TryParse);
        }

        public static bool TryParseNullableLong(string s, NumberStyles numericStyles, out long? result)
        {
            return TryParseNullable(s, numericStyles, out result, long.TryParse);
        }

        public static bool TryParseNullableBoolean(string s, out bool? result)
        {
            return TryParseNullable(s, out result, bool.TryParse);
        }

        public static bool TryParseNullableDateTime(string s, out DateTime? result)
        {
            return TryParseNullable(s, out result, DateTime.TryParse);
        }

        //An override for the DateTime to allow us to pass in a pattern
        public static bool TryParseNullableDateTime(string s, string pattern, out DateTime? result)
        {

            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return true;
            }
            else if (string.IsNullOrEmpty(pattern))
            {
                DateTime temp;
                bool success = DateTime.TryParse(s, out temp);
                result = temp;
                return success;
            }
            else
            {
                DateTime temp;
                bool success = DateTime.TryParseExact(s, pattern, null, System.Globalization.DateTimeStyles.None, out temp);
                result = temp;
                return success;
            }

        }

    }
}