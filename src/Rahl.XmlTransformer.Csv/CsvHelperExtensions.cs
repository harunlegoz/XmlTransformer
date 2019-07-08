using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Rahl.XmlTransformer.Csv
{
    public static class CsvHelperExtensions
    {
        public static void SetOutputFromElement<T>(this CsvWriter row, XElement parentElement, string elementName,
            string outputName = null, string format = null, Func<string, string> coerceRaw = null, Func<T, T> coerceValue = null, NumberStyles? numberStyles = null)
        {
            var val = GetValue<T>(parentElement, elementName, format, coerceRaw, null, numberStyles);

            row.WriteField(val);
        }

        public static void SetOutput<T>(this CsvWriter row, XElement parentElement, string elementName,
            IList<string> errors, string outputName = null, string format = null, Func<string, string> coerceRaw = null, Func<T, T> coerceValue = null, NumberStyles? numberStyles = null)
        {
            var originalValue = parentElement.GetValue(elementName);
            var extractedValue = GetValue(parentElement, elementName, format, coerceRaw, coerceValue);

            if (UnableToConvertFieldValue(originalValue, extractedValue))
            {
                errors.Add($"Unable to convert field {elementName} with value: {originalValue}");
            }

            row.WriteField(extractedValue);
        }

        private static T GetValue<T>(XElement parentElement, string elementName, string format, Func<string, string> coerceRaw,
            Func<T, T> coerceValue, NumberStyles? numberStyles = null)
        {
            T extractedValue = parentElement.GetValue<T>(elementName, format, coerceRaw, numberStyles);
            if (coerceValue != null)
            {
                extractedValue = coerceValue(extractedValue);
            }

            return extractedValue;
        }

        public static void SetDateOuput(this CsvWriter row, XElement parentElement, string elementName,
            IList<string> errors, string outputName = null, string format = null)
        {
            string originalValue = parentElement.GetValue(elementName);
            DateTime? extractedValue = ParseNullableUsDateTime(originalValue);

            if (UnableToConvertFieldValue(originalValue, extractedValue))
            {
                errors.Add($"Unable to convert field {elementName} with value: {originalValue}");
            }

            row.WriteField(extractedValue);
        }

        public static void SetOutPutError(this CsvWriter row, string elementName, IList<string> errorList, string errorRowId)
        {
            var errorMsg = errorList.Count > 0 ? $"{errorRowId}. {string.Join("|", errorList)}" : null;
            row.WriteField(errorMsg);
        }

        private static bool UnableToConvertFieldValue<T>(string originalValue, T extractedValue)
        {
            return !string.IsNullOrWhiteSpace(originalValue) && extractedValue == null;
        }

        private static DateTime? ParseNullableUsDateTime(string s)
        {
            try
            {
                return DateTime.Parse(s, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}