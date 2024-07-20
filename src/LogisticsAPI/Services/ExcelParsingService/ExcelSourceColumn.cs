namespace LogisticsAPI.Services.ExcelParsingService
{
  internal readonly struct ExcelSourceColumn(int colIndex, string name, ParsingTypeCode parsingTypeCode, bool isValueRequired, string? filterString = null)
    {
        #region Properties
        public int ColIndex { get; } = colIndex;
        public string Name { get; } = name;
        public ParsingTypeCode ParsingTypeCode { get; } = parsingTypeCode;
        public bool IsValueRequired { get; } = isValueRequired;
        public string? FilterString { get; } = filterString;
        #endregion
    }
}