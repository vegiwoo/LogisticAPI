namespace LogisticsAPI.Services.ExcelService.Items
{
    public class DataColumnForParsing(int index, string name, Type dataType, bool isValueRequired, string? filterString = null)
    {
        #region Properties
        public int Index { get; } = index;
        public string Name { get; } = name;
        public Type DataType { get; } = dataType;
        public bool IsValueRequired { get; } = isValueRequired;
        public string? FilterString { get; } = filterString;
        #endregion
    }
}