using System.Reflection;

namespace LogisticsAPI.Services.ExcelService.Items.ParsingRowEntity
{
    /// <summary>
    /// An entity for parsing data from an Excel table.
    /// </summary>
    public interface IParsingRowEntity
    {
        public PropertyInfo[] PropertyInfos { get; }
        public static List<string> DateOnlyFormats { get; } = ["dd.MM.yyyy HH:mm", "dd.MM.yyyy HH:mm:ss"];
    }
}