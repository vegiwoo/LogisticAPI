using LogisticsAPI.Models;

namespace LogisticsAPI.Services.ExcelService.Items
{
    public class TableColumnPrototype(int columnIndex, string name, ParsingTypeCode dataTypeCode, bool isValueRequired, string? filterString = null)
    {
        #region Properties
        public int ColumnIndex { get; } = columnIndex;
        public string Name { get; } = name;
        public ParsingTypeCode DataType { get; } = dataTypeCode;
        public bool IsValueRequired { get; } = isValueRequired;
        public string? FilterString { get; } = filterString;
        #endregion
    }
}