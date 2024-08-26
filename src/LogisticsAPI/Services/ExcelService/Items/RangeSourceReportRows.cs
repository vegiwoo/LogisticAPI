namespace LogisticsAPI.Services.ExcelService.Items 
{
    /// <summary>
    /// Represents a range of source report rows.
    /// </summary>
    public class RangeSourceReportRows(List<TableColumnPrototype> dataColumnsForParsing, string? key = null, (string key, int offset)? terminatingStringValue = null)
    {
        #region Properties
        /// <summary>
        /// String key with name of range, can be empty (for example, if there is only one range).
        /// </summary>
        public string? Key { get; } = key;

        /// <summary>
        /// Terminating string value from which index of the last row is calculated (can be empty)
        /// </summary>
        public (string key, int offset)? TerminatingStringValue { get; } = terminatingStringValue;

        /// <summary>
        /// Collection of SourceColumn objects (describe cource columns of range)
        /// </summary>
        public List<TableColumnPrototype> DataColumnsForParsing { get; } = dataColumnsForParsing;

        /// <summary>
        /// A collection of range row indexes.
        /// </summary>
        public IEnumerable<int>? RangeRowIndexes { get; set; } = [];

        /// <summary>
        /// A collection of data on read rows (each row is a dictionary of values).
        /// </summary>
        public List<Dictionary<string, ItemForParsing>> DataOnReadRowsCollection { get; private set; } = [];

        #endregion

        #region Functionality
        public void SetRange(int startRowIndex, int count) =>
            RangeRowIndexes = Enumerable.Range(startRowIndex, count);

        public void SetValues(Dictionary<string, ItemForParsing> values) => DataOnReadRowsCollection.Add(values);
        #endregion Functionality
    }
}