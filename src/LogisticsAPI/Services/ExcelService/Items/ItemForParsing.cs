using LogisticsAPI.Models;

namespace LogisticsAPI.Services.ExcelService.Items
{
    public class ItemForParsing(string? value, ParsingTypeCode valueTypecode, bool isMaybeNullable = false)
    {
        #region Properties
        public string? Value { get; } = value;
        public ParsingTypeCode ValueTypecode { get; } = valueTypecode;
        public bool IsValueMaybeNullable { get; } = isMaybeNullable;
        #endregion
    }
}