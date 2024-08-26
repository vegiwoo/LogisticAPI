namespace LogisticsAPI.Services.ExcelService.Items
{
    public class ItemForParsing(string? value, Type valueType, bool isMaybeNullable = false)
    {
        #region Properties
        public string? Value { get; } = value;
        public Type ValueType { get; } = valueType;
        public bool IsMaybeNullable { get; } = isMaybeNullable;
        #endregion
    }
}