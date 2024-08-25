
using LogisticsAPI.Services.ExcelService.Items;
using LogisticsAPI.Services.FileService;
using OfficeOpenXml;


namespace LogisticsAPI.Services.ExcelService
{
    public class ExcelService : IExcelService
    {

        #region Variables and constants 
        const int DEFAULT_PARSING_STEP = 1;
        #endregion

        #region Properties
        public Dictionary<FileContext, (string worksheetName, List<RangeSourceReportRows> ranges)> DataColumnsForParsing {get; set;} = new() 
        {
            {
                FileContext.СlientsSKUs, ("Справочник", new List<RangeSourceReportRows> 
                {
                    new(
                    [
                        //new(1, "SKUNumber", typeof(int), true),
                        //new(2, "SKUName", typeof(string), true),
                        //new(3, "SKUSizes", typeof(string), false),
                        new(4, "ServiceId", typeof(int), true),
                        new(5, "Name", typeof(string), false),
                        new(6, "Nick", typeof(string), false),
                        new(7, "Phone", typeof(string), true)
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

        #endregion
    }
}