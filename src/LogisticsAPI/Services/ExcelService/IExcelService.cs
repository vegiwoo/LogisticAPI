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

        /// <summary>
        /// Gets data from row ranges in specified Excel worksheet.
        /// </summary>
        /// <param name="workSheet">Specified Excel worksheet</param>
        /// <param name="reportRowsRanges">List of ranges.</param>
        public void GetRawDataFromExcelRows(in ExcelWorksheet workSheet, ref List<RangeSourceReportRows> reportRowsRanges);

        /// Gets data from a range row in specified Excel worksheet.
        /// </summary>
        /// <param name="workSheet">Specified Excel worksheet.</param>
        /// <param name="rowIndex">Row index.</param>
        /// <param name="sourceColumns">A list with a description of source cells in a row.</param>
        /// <param name="values">Dictionary of retrieved values by row.</param>
        /// <returns>Boolean flag for success of completed operation.</returns>
        /// <remarks><i>New version, tested</i></remarks>
        public bool GetValuesFromRow(in ExcelWorksheet workSheet, int rowIndex, in List<TableColumnPrototype> sourceColumns, out Dictionary<string, ItemForParsing>? values);

        /// <summary>
        /// Checks for presence of addresses in a row of an Excel table. 
        /// </summary>
        /// <param name="workSheet">Excel workbook worksheet.</param>
        /// <param name="rowIndex">Row index.</param>
        /// <param name="sourceColumns">Column prototypes for creating an address (row/column).</param>
        /// <returns>Check flag.</returns>
         public bool CheckPresenceAddressesInRow(in ExcelWorksheet workSheet, int rowIndex, in List<TableColumnPrototype> sourceColumns);

    }
}