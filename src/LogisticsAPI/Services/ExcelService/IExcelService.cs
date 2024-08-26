using LogisticsAPI.Services.ExcelService.Items;
using LogisticsAPI.Services.FileService;
using OfficeOpenXml;

namespace LogisticsAPI.Services.ExcelService
{
    public interface IExcelService
    {
        public Dictionary<FileContext, (string worksheetName, List<RangeSourceReportRows> ranges)> DataColumnsForParsing {get; set;}

        /// <summary>
        /// Returns a Worksheet from given Excel file named Worksheet.
        /// </summary>
        /// <param name="package">Source file Excel.</param>
        /// <param name="name">Worksheet name.</param>
        /// <param name="worksheet">Worksheet (if found by name) or null.</param>
        /// <returns>Worksheet search flag.</returns>
        public bool GetWorksheetByName(in ExcelPackage package, string name, out ExcelWorksheet? worksheet);

        /// <summary>
        /// Creates ranges of row indices.
        /// </summary>
        /// <param name="worksheet">Рабочий лист книги Excel.</param>
        /// <param name="rangeSourceReportRows">Исзодная коллекция элементов типа RangeSourceReportRows.</param>
        /// <param name="defaultStep">Шаг по-умолчанию.</param>
        /// <param name="termination">Завершающее значение.</param>
        public void CreatesRangesOfRowIndices(in ExcelWorksheet worksheet, ref List<RangeSourceReportRows> rangeSourceReportRows, int defaultStep, (string rowStringKey, int offset)? termination);

        /// <summary>
        /// Gets ranges of rows from an Excel sheet.
        /// </summary>
        /// <param name="worksheet">Excel worksheet.</param>
        /// <param name="rangeSourceReportRows">Range prototypes.</param>
        /// <param name="startRangeRowIndex">Start index.</param>
        public void GetRangesRowsFromExcelWorksheet(in ExcelWorksheet worksheet, ref List<RangeSourceReportRows> rangeSourceReportRows, int startRangeRowIndex = 1);

        /// <summary>
        /// Returns index of row from an Excel workbook by string value (key).
        /// </summary>
        /// <param name="key">String key to search for a row.</param>
        /// <param name="worksheet">Excel worksheet for searching.</param>
        /// <returns>Integer index of row or null.</returns>
        public bool GetRowIndexFromExcelWorksheetByStringValue(string key, in ExcelWorksheet worksheet, out int? rowInex);
    }
}