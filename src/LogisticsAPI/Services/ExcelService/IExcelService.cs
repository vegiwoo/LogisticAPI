using LogisticsAPI.Services.FileService;
using OfficeOpenXml;

namespace LogisticsAPI.Services.ExcelService
{
    public interface IExcelService
    {
        public Dictionary<FileContext, (string worksheetName, List<DataColumnForParsing> dataColumns)> DataColumnsForParsing {get; set;}

        /// <summary>
        /// Returns a Worksheet from given Excel file named Worksheet.
        /// </summary>
        /// <param name="package">Source file Excel.</param>
        /// <param name="name">Worksheet name.</param>
        /// <param name="worksheet">Worksheet (if found by name) or null.</param>
        /// <returns>Worksheet search flag.</returns>
        public bool GetWorksheetByName(in ExcelPackage package, string name, out ExcelWorksheet? worksheet);
    }
}