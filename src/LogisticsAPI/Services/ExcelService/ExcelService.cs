<<<<<<< HEAD
using LogisticsAPI.Services.FileService;
=======
using System.Diagnostics;
using LogisticsAPI.Services.FileService;
using OfficeOpenXml;
>>>>>>> 5155116 (Developing the CreateClients POST route logic)

namespace LogisticsAPI.Services.ExcelService
{
    public class ExcelService : IExcelService
    {
<<<<<<< HEAD
        private readonly Dictionary<FileContext, List<DataColumnForParsing>> DataColumnsForParsing = new() 
        {
            {
                FileContext.СlientsSKUs, new List<DataColumnForParsing> 
=======
        public Dictionary<FileContext, (string worksheetName, List<DataColumnForParsing> dataColumns)> DataColumnsForParsing {get; set;} = new() 
        {
            {
                FileContext.СlientsSKUs, ("Справочник", new List<DataColumnForParsing> 
>>>>>>> 5155116 (Developing the CreateClients POST route logic)
                {
                    //new(1, "SKUNumber", typeof(int), true),
                    //new(2, "SKUName", typeof(string), true),
                    //new(3, "SKUSizes", typeof(string), false),
                    new(4, "ServiceId", typeof(int), true),
                    new(5, "Name", typeof(string), false),
                    new(6, "Nick", typeof(string), false),
                    new(7, "Phone", typeof(string), true)
<<<<<<< HEAD
                }
            }
        };
=======
                })
            }
        };

        public bool GetWorksheetByName(in ExcelPackage package, string name, out ExcelWorksheet? worksheet)
        {
            worksheet = package.Workbook.Worksheets.First(sh => string.Equals(sh.Name.Trim().ToLower(),name.Trim().ToLower()));
            return worksheet is not null;
        }
>>>>>>> 5155116 (Developing the CreateClients POST route logic)
    }
}