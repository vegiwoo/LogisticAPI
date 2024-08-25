using LogisticsAPI.Services.FileService;

namespace LogisticsAPI.Services.ExcelService
{
    public class ExcelService : IExcelService
    {
        private readonly Dictionary<FileContext, List<DataColumnForParsing>> DataColumnsForParsing = new() 
        {
            {
                FileContext.СlientsSKUs, new List<DataColumnForParsing> 
                {
                    //new(1, "SKUNumber", typeof(int), true),
                    //new(2, "SKUName", typeof(string), true),
                    //new(3, "SKUSizes", typeof(string), false),
                    new(4, "ServiceId", typeof(int), true),
                    new(5, "Name", typeof(string), false),
                    new(6, "Nick", typeof(string), false),
                    new(7, "Phone", typeof(string), true)
                }
            }
        };
    }
}