using System.Timers;

namespace LogisticsAPI.Services.FileService
{
    public enum FileType 
    {
        Excel
    }

     public enum FileContext 
    {
        СlientsSKUs
    }

    internal class FileService : IFileService
    {
        #region Variables and constants

        private static readonly string _tempDirectory = Path.GetTempPath();

        private static readonly string[] _allowedExcelFileExtensions = [".xls","xlsx"];
        private readonly Dictionary<FileType, string[]> _allowedFileExtensions = new()
        {
            {FileType.Excel, _allowedExcelFileExtensions }
        };
        private readonly Dictionary<FileContext, string[]> _allowedFileNames = new()
        {
            {FileContext.СlientsSKUs, ["Справочник (WB)","Справочник (Ozon)"] }
        };

        #endregion

        #region Functionality
        public bool CheckFileExtension(FileType fileType, in string? file)
        {
            if(string.IsNullOrEmpty(file)) 
            {
                // логировать Имя класса + "File extension check cannot be empty."
                return false;
            }

            string[] allowedFileExtensions = this._allowedFileExtensions
                .FirstOrDefault(el => el.Key == fileType).Value;

            if(allowedFileExtensions is null || allowedFileExtensions.Length == 0) 
            {
                // логировать Имя класса + "There are no valid extensions defined for specified file type."
                return false;
            }

            return allowedFileExtensions.Contains(file);
        }

        public bool CheckFileName(FileContext fileContext, in string fileIName)
        {
            if(string.IsNullOrEmpty(fileIName)) 
            {
                // логировать Имя класса + "File name check cannot be empty."
                return false;
            }

            string[] allowedFileNames = this._allowedFileNames
                .FirstOrDefault(el => el.Key == fileContext).Value;

            if(allowedFileNames is null || allowedFileNames.Length == 0) 
            {
                // логировать Имя класса + "There are no valid namrs defined for specified file context."
                return false;
            }

            return allowedFileNames.Contains(fileIName);
        }

        public bool CheckFileIsNotEmpty(ref FileInfo fileInfo) => fileInfo.Length > 0;
        
        #endregion
    }
}