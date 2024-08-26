using System.Diagnostics;
using System.Reflection;
using LogisticsAPI.Models;

namespace LogisticsAPI.Services.ExcelService.Items.ParsingRowEntity
{
    /// <summary>
    /// Element for parsing data from a row of an Excel workbook (inherited by a specific class for parsing).
    /// </summary>
    public abstract class ParsingRowEntity : IParsingRowEntity
    {
        #region Variables and constants
        protected const string SPACE_SEPARATOR = "\u0020";
        protected readonly List<TypeCode> baseTypeCodes = [TypeCode.Double, TypeCode.Int32, TypeCode.DateTime, TypeCode.String];

        protected Type DoubleType = typeof(double);
        #endregion

        #region Properties
        public PropertyInfo[] PropertyInfos { get; private set; }
        #endregion

        #region  Constructors
        public ParsingRowEntity(in Dictionary<string, ItemForParsing> parsingElements)
        {
            PropertyInfos = GetType().GetProperties();

            try
            {
                ParseValues(parsingElements);
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
                // TODO: Логировать ex.Message
                throw;
            }
        }
        #endregion

        #region Functionality
        protected static void ThrowExceptionOnArgument(string valueKey) =>
            throw new ArgumentException($"Key value not provided: '{valueKey}'");

        protected static void ThrowExceptionCast(string valueKey, string value, Type type) =>
            throw new InvalidCastException($"Element with key '{valueKey}' and value '{value}' cannot be casting to type {type}.");
        
        /// <summary>
        /// Parses data from a data dictionary into class properties.
        /// </summary>
        /// <param name="parsingElements">Dict of data to parse (conceptually represents a row from an Excel workbook).</param>
        protected void ParseValues(in Dictionary<string, ItemForParsing> parsingElements)
        {
            foreach (var parsingElement in parsingElements)
            {
                // Get target property
                PropertyInfo? targetProperty = PropertyInfos
                    .FirstOrDefault(p => p.Name == parsingElement.Key);
                    
                if (targetProperty is null)
                    ThrowExceptionOnArgument(parsingElement.Key);
                    // TODO: Логировать ошибку

                try
                {
                    ParseValue(in parsingElement, ref targetProperty!);
                }
                catch (System.Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    // TODO: Логировать ошибку
                    throw;
                }
            }
        }

        /// <summary>
        /// Parses data from a dictionary element into a class property.
        /// </summary>
        /// <param name="element">Dictionary element (conceptually represents a cell of Excel spreadsheet data).</param>
        /// <param name="property"A reference to the target property of class.></param>
        /// <exception cref="Exception"></exception>
        private void ParseValue(in KeyValuePair<string, ItemForParsing> element, ref PropertyInfo property)
        {
            string? stringValueOfElement = element.Value.Value;
            bool isValueMaybeNullable = element.Value.IsValueMaybeNullable;

            if (string.IsNullOrEmpty(stringValueOfElement))
            {
                if (!isValueMaybeNullable)
                    throw new Exception($"Parsing value with key '{element.Key}' cannot be null.");
                    // TODO: Логировать ошибку

                stringValueOfElement = string.Empty;
            }
            else
            {
                stringValueOfElement = stringValueOfElement.Trim();
            }

            if(ParsingValueFromString(stringValueOfElement, element.Value.ValueTypecode, isValueMaybeNullable, out object? value) 
                && value is not null) 
            {
               if(property.CanWrite) 
                {
                    switch (element.Value.ValueTypecode)
                    {
                        case ParsingTypeCode.DoubleTypeCode: 
                            property.SetValue(this, (double)value!);
                            break;

                        case ParsingTypeCode.IntTypeCode:
                            property.SetValue(this, (int)value!);
                            break;

                        //22.03.2024 14:00
                        case ParsingTypeCode.DataOnlyTypeCode:
                            property.SetValue(this, (DateOnly)value!);
                            break;

                        case ParsingTypeCode.StringTypeCode:
                            property.SetValue(this, (string)value!);
                            break;
                    }
               }
            }
        }
        
        /// <summary>
        /// Directly parses the value from string.
        /// </summary>
        /// <param name="sourceStringValue">Original string value.</param>
        /// <param name="targetValueTypeCode">Target value type code.</param>
        /// <param name="isMaybeNullable">Value may be nullable.</param>
        /// <param name="value">Resulting target value or null.</param>
        /// <returns>Flag parsing value.</returns>
        /// <exception cref="NotImplementedException">Exception - Parsing case is not implemented.</exception>
        public static bool ParsingValueFromString(string sourceStringValue, ParsingTypeCode targetValueTypeCode, bool isMaybeNullable,  out object? value) 
        {
            value = null;

            switch (targetValueTypeCode)
            {
                case ParsingTypeCode.IntTypeCode:

                    if (isMaybeNullable && string.IsNullOrEmpty(sourceStringValue))
                    {
                        value = 0;
                    }
                    else
                    {
                        value = int.TryParse(sourceStringValue, out int intValue) ? intValue : null;
                    }

                    break;

                case ParsingTypeCode.DoubleTypeCode:

                    if (isMaybeNullable && string.IsNullOrEmpty(sourceStringValue))
                    {
                        value = 0.00;
                    }
                    else
                    {
                        if (sourceStringValue.Split(SPACE_SEPARATOR).Length == 2)
                            sourceStringValue = sourceStringValue.Split(SPACE_SEPARATOR)[0];

                        value = double.TryParse(sourceStringValue, out double doubleValue) ? doubleValue : null;
                    }
                    break;

                case ParsingTypeCode.DataOnlyTypeCode:

                    if (isMaybeNullable && string.IsNullOrEmpty(sourceStringValue))
                    {
                        value = new DateOnly(0001, 01, 01);
                    }
                    else
                    {
                        value = DateOnly.TryParseExact(sourceStringValue, IParsingRowEntity.DateOnlyFormats.ToArray(), out DateOnly dateOnlyValue) ? dateOnlyValue : null;
                    }
                    break;

                case ParsingTypeCode.StringTypeCode:

                    if (isMaybeNullable && string.IsNullOrEmpty(sourceStringValue))
                    {
                        value = String.Empty;
                    }
                    else
                    {
                        value = string.IsNullOrEmpty(sourceStringValue) ? sourceStringValue!.Trim() : null;
                    }
                    break;

                case ParsingTypeCode.SKURansomsStatus:

                    value = sourceStringValue switch
                    {
                        "Доставлен на ПВЗ" => SKURansomsStatus.Delivered,
                        "Забран с ПВЗ" => SKURansomsStatus.WithDrawn,
                        "Отменен" => SKURansomsStatus.Canceled,
                        "В пути" => SKURansomsStatus.OnWay,
                        _ => throw new NotImplementedException()
                        // TODO: Логировать ошибку по кейсу
                    };

                    break;

                case ParsingTypeCode.Marketplace:

                    value = sourceStringValue switch
                    {
                        "wb" or "WB" or "Wb" or "Wildberries" => Marketplace.WB,
                        "oz" or "Oz" or "OZ" or "Ozon" => Marketplace.OZ,
                        _ => throw new NotImplementedException(),
                        // TODO: Логировать ошибку по кейсу
                    };
                    break;

                default:
                    throw new NotImplementedException();
                    // TODO: Логировать ошибку по кейсу
            }

            return value is not null;
        }
        
        #endregion
    }
}