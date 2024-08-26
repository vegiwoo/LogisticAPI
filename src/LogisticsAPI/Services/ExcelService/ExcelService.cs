using System.Diagnostics;
using LogisticsAPI.Models;
using LogisticsAPI.Services.ExcelService.Items;
using LogisticsAPI.Services.ExcelService.Items.ParsingRowEntity;
using LogisticsAPI.Services.FileService;
using OfficeOpenXml;

namespace LogisticsAPI.Services.ExcelService
{
    public class ExcelService : IExcelService
    {
        #region Variables and constants
        private const int  DEFAULT_PARSING_STEP = 1;
        #endregion

        #region Properties 
        public Dictionary<FileContext, (string worksheetName, List<RangeSourceReportRows> ranges)> DataColumnsForParsing {get; set;} = new()
        {
            {
                FileContext.СlientsSKUs, ("Справочник", new List<RangeSourceReportRows> 
                {
                    new([
                        //new(1, "SKUNumber", typeof(int), true),
                        //new(2, "SKUName", typeof(string), true),
                        //new(3, "SKUSizes", typeof(string), false),
                        new(4, "ServiceId", ParsingTypeCode.IntTypeCode, true),
                        new(5, "Name", ParsingTypeCode.StringTypeCode, false),
                        new(6, "Nick", ParsingTypeCode.StringTypeCode, false),
                        new(7, "Phone", ParsingTypeCode.StringTypeCode, true)
                    ])
                })
            }
        };
        #endregion

        #region Functionality
        public void GetRangesRowsFromExcelWorksheet(in ExcelWorksheet worksheet, ref List<RangeSourceReportRows> rangeSourceReportRows, int startRangeRowIndex = 1)
        {
            IEnumerable<RangeSourceReportRows>? rangesWithKeys = rangeSourceReportRows
                .Where(rs => !string.IsNullOrEmpty(rs.Key));

            RangeSourceReportRows? rangeWithTermination = rangeSourceReportRows
                .SingleOrDefault(rs => rs.TerminatingStringValue.HasValue);

            bool isOneRangeWithoutKeysAndTerminations = rangeSourceReportRows.Count == 1 && 
                (rangesWithKeys is null || !rangesWithKeys.Any()) && 
                rangeWithTermination is null;

            // Range is one, no key and no termination value
            if(isOneRangeWithoutKeysAndTerminations)
            { 
                var start = startRangeRowIndex + DEFAULT_PARSING_STEP;
                var count = worksheet.Dimension.End.Row - DEFAULT_PARSING_STEP;
                rangeSourceReportRows.First().SetRange(start,count);
            } 
            // There is more than one range (e.g. Remainig)
            else if(rangeSourceReportRows.Count > 1)
            {
                CreatesRangesOfRowIndices(in worksheet, ref rangeSourceReportRows, DEFAULT_PARSING_STEP, rangeWithTermination?.TerminatingStringValue);
            }
        }

        public bool GetRowIndexFromExcelWorksheetByStringValue(string key, in ExcelWorksheet worksheet, out int? rowInex)
        {
            rowInex =  worksheet.Cells.FirstOrDefault(c => c.Value != null && c.Value.ToString()?.Trim().ToLower() == key.Trim().ToLower())?.Start.Row;
            return rowInex.HasValue;
        }

        public void CreatesRangesOfRowIndices(in ExcelWorksheet worksheet, ref List<RangeSourceReportRows> rangeSourceReportRows, int defaultStep, (string rowStringKey, int offset)? termination)
        {
            List<RangeSourceReportRows> outputRanges = [];

            // Get existing keys from templates 
            List<string>? keysInTemplateList = rangeSourceReportRows
                .Where(el => !string.IsNullOrEmpty(el.Key))
                .Select(el => el.Key!)
                .ToList();

            // Get existing elements in table by key
            List<(string key, int index)> existingElements = new();
            for (int i = 0; i < keysInTemplateList.Count; i++)
            {
                var key = keysInTemplateList[i];
                if(!string.IsNullOrEmpty(key) && 
                    GetRowIndexFromExcelWorksheetByStringValue(key, in worksheet, out int? rowIndex) && 
                    rowIndex.HasValue)
                {
                    existingElements.Add((key, rowIndex.Value));
                }
            }

            // Make LinkedList from existing elements in table  
            LinkedList<(string key, int index)> existingElementsLinkedList = new(existingElements);

            int? count = null, start = null;

            // Iterate linked list
            var currentNode = existingElementsLinkedList.First;

            while(currentNode != null)
            {
                (string key, int index) = currentNode.Value;

                start = index + defaultStep;

                if(currentNode.Next != null) 
                {
                    (string nextKey, int nextIndex) = currentNode.Next!.Value;
                    count = nextIndex - start.Value;
                } 
                else
                {
                    if(termination is not null) 
                    {
                        var terminationKey = termination.Value.rowStringKey;
                        var terminationOffset = termination.Value.offset;

                        if(!string.IsNullOrEmpty(terminationKey) && 
                           GetRowIndexFromExcelWorksheetByStringValue(terminationKey, in worksheet, out int? terminationIndex) && 
                           terminationIndex.HasValue)
                        {
                            count = terminationIndex - start.Value - terminationOffset;
                        } 
                        else 
                        {
                            throw new ArgumentException("Cannot get index of termination row.");
                        }
                    } 
                    else 
                    {
                        throw new ArgumentException("Multiple ranges must contain a termination row.");
                    } 
                }

                // Set range
                var targetTemplate = rangeSourceReportRows
                    .SingleOrDefault(el => string.Equals(el.Key, key));

                if(targetTemplate is not null && 
                   start.HasValue && 
                   count.HasValue) 
                {
                    targetTemplate.SetRange(start.Value, count.Value);
                    outputRanges.Add(targetTemplate);
                }

                // Next step
                currentNode = currentNode!.Next;
            }

            // Return
            rangeSourceReportRows = outputRanges;
        }

        public bool GetWorksheetByName(in ExcelPackage package, string name, out ExcelWorksheet? worksheet)
        {
            worksheet = package.Workbook.Worksheets.First(sh => string.Equals(sh.Name.Trim().ToLower(),name.Trim().ToLower()));
            return worksheet is not null;
        }

        public void GetRawDataFromExcelRows(in ExcelWorksheet workSheet, ref List<RangeSourceReportRows> reportRowsRanges)
        {
            // Iterating Ranges
            for (int i = 0; i < reportRowsRanges.Count; i++)
            {
                RangeSourceReportRows? reportRowsRange = reportRowsRanges[i];

                if (reportRowsRange is null || reportRowsRange.DataColumnsForParsing is null) continue;

                // Iterating Rows 
                var rowsIndexRange = reportRowsRange.RangeRowIndexes?.ToList();
                var currentSourceCells = reportRowsRange.DataColumnsForParsing;

                for (int j = 0; j < rowsIndexRange?.Count; j++)
                {
                    var rowIndex = rowsIndexRange[j];

                    if (GetValuesFromRow(in workSheet, rowIndex, in currentSourceCells, out Dictionary<string, ItemForParsing>? values) &&
                        values is not null)
                    {
                        reportRowsRange.SetValues(values);
                        reportRowsRanges![i] = reportRowsRange;
                    }
                }
            }
        }

        /// <summary>
        /// Gets data from a row in an Excel workbook table.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowIndex">Index of row containing data.</param>
        /// <param name="tableColumnPrototypes">Table column prototypes.</param>
        /// <param name="values">Dictionary of data obtained from a string</param>
        /// <returns>Flag of check execution.</returns>
        /// <exception cref="ArgumentException">Exceptions during execution.</exception>
        public bool GetValuesFromRow(in ExcelWorksheet workSheet, int rowIndex, in List<TableColumnPrototype> tableColumnPrototypes, out Dictionary<string, ItemForParsing>? values)
        {
            values = null;

            if (!CheckPresenceAddressesInRow(in workSheet, rowIndex, in tableColumnPrototypes) ||
                !СheckingValuesInRequiredCells(in workSheet, rowIndex, in tableColumnPrototypes) ||
                !FilterMatchingCheck(in workSheet, rowIndex, in tableColumnPrototypes, out int filterCellsCount) ||
                !FindSorceCellsByType(in tableColumnPrototypes, SourceCellsType.NonFiltration, out List<TableColumnPrototype>? dataСellsNonFilters) ||
                dataСellsNonFilters is null ||
                dataСellsNonFilters!.Count == 0)
            {
                return false;
            }

            // Getting values
            values = [];
            foreach (var dataCell in dataСellsNonFilters)
            {
                var value = workSheet.Cells[rowIndex, dataCell.ColumnIndex].Value;

                if (value is null)
                {
                    if (dataCell.IsValueRequired)
                        throw new ArgumentException($"A table cell does not contain a <b>required value</b> for name: '{dataCell.Name}'");
                        // TODO: Логировать исколючение.
                    else
                        value = string.Empty;
                }

                var valueToString = value.ToString();
                if (valueToString is not null)
                    values.Add(dataCell.Name, new(valueToString, dataCell.DataType, !dataCell.IsValueRequired));
            }

            return Equals(values?.Count, tableColumnPrototypes.Count - filterCellsCount);
        }

        public bool CheckPresenceAddressesInRow(in ExcelWorksheet workSheet, int rowIndex, in List<TableColumnPrototype> tableColumnPrototypes)
        {
            foreach (var tableColumnPrototype in tableColumnPrototypes)
            {
                ExcelRange? address = workSheet.Cells[rowIndex, tableColumnPrototype.ColumnIndex];
                if (address is null) return false;

                var addressValue = address.Value;
                if(addressValue is not null && !string.IsNullOrEmpty(addressValue.ToString())) 
                {
                    return ParsingRowEntity.ParsingValueFromString(addressValue.ToString()!, tableColumnPrototype.DataType, !tableColumnPrototype.IsValueRequired, out object? value) && 
                        value is not null; 
                }
                else 
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks for data in "required" cells.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowIndex">Index of the row containing data.</param>
        /// <param name="tableColumnPrototypes">A prototype collection of a string.</param>
        /// <returns>Flag of check execution.</returns>
        private static bool СheckingValuesInRequiredCells(in ExcelWorksheet workSheet, int rowIndex, in List<TableColumnPrototype> tableColumnPrototypes)
        {
            if(!FindSorceCellsByType(in tableColumnPrototypes, SourceCellsType.Required, out List<TableColumnPrototype>? requiredCells) && 
                requiredCells is null || 
                requiredCells?.Count == 0)
            {
                return true;
            }
            else 
            {
                var requiredCellValues = new List<object>();
                foreach (var sourceCell in requiredCells!)
                {
                    var cell = workSheet.Cells[rowIndex, sourceCell.ColumnIndex];
                    var value = cell.Value;
                    if (cell is null || value is null)
                    {
                        return false;
                    } 
                    else 
                    {
                        requiredCellValues.Add(value);
                    }
                }

                return requiredCellValues.All(v => v is not null);
            }
        }

        /// <summary>
        /// Finds cells in a column prototype by their type.
        /// </summary>
        /// <param name="tableColumnPrototypes">Prototype collection of columns.</param>
        /// <param name="sourceCellsType">Type to search for cells.</param>
        /// <param name="cells">Type to search for cells.</param>
        /// <returns>Boolean search flag.</returns>
        private static bool FindSorceCellsByType(in List<TableColumnPrototype> tableColumnPrototypes, SourceCellsType sourceCellsType, out List<TableColumnPrototype>? cells)
        {
            cells = sourceCellsType switch
            {
                SourceCellsType.Required => tableColumnPrototypes.Where(sc => sc.IsValueRequired).ToList(),
                SourceCellsType.Filtration => tableColumnPrototypes.Where(sc => !string.IsNullOrEmpty(sc.FilterString)).ToList(),
                SourceCellsType.NonFiltration => tableColumnPrototypes.Where(sc => string.IsNullOrEmpty(sc.FilterString)).ToList(),
                _ => null,
            };

            return cells is not null && cells.Count != 0;
        }

        /// <summary>
        /// Checks whether values ​​in source table match specified filter values.
        /// </summary>
        /// <param name="workSheet">Excel worksheet.</param>
        /// <param name="rowIndex">Index of row containing data.</param>
        /// <param name="tableColumnPrototypes">Table column prototypes.</param>
        /// <param name="filterSourceCellsCount">Number of columns for filtering.</param>
        /// <returns>Flag of check execution.</returns>
         private static bool FilterMatchingCheck(in ExcelWorksheet workSheet, int rowIndex, in List<TableColumnPrototype> tableColumnPrototypes, out int filterSourceCellsCount)
        {
            filterSourceCellsCount = 0;

            if (!FindSorceCellsByType(in tableColumnPrototypes, SourceCellsType.Filtration, out List<TableColumnPrototype>? filterSourceCells)) 
            {
                return true;
            } 
            else 
            {
                filterSourceCellsCount = filterSourceCells!.Count;
                foreach (var filterSourceCell in filterSourceCells)
                {
                    if (!IsStringValueMatchByFilter(filterSourceCell.FilterString!, workSheet.Cells[rowIndex, filterSourceCell.ColumnIndex].Value))
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Checks for a match of string values ​​(by filter).
        /// </summary>
        /// <param name="filter">Filter value.</param>
        /// <param name="value">Value in an Excel table cell.</param>
        /// <returns></returns>
        private static bool IsStringValueMatchByFilter(string filter, object value) =>
            value is not null &&
            !string.IsNullOrEmpty(value.ToString()) &&
            string.Equals(filter.Trim().ToLower(), value.ToString()!.Trim().ToLower());

        #endregion
    }
}