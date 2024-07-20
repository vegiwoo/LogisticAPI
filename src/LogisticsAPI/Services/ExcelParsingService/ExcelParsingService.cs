namespace LogisticsAPI.Services.ExcelParsingService
{
    internal class ExcelParsingService
    {
        private static Dictionary<ParsingTypeCode, Type> TypeMappingTable { get; } = new()
        {
            { ParsingTypeCode.StringTypeCode, typeof(string) },
            { ParsingTypeCode.IntTypeCode, typeof(int) },
            { ParsingTypeCode.DoubleTypeCode, typeof(double) },
            { ParsingTypeCode.DataOnlyTypeCode, typeof(DateOnly) }
        };
    }
}