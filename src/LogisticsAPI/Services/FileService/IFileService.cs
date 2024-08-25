namespace LogisticsAPI.Services.FileService
{
    public interface IFileService
    {
        /// <summary>
        /// Check validity of file extension according to its type. 
        /// </summary>
        /// <param name="fileType">File type.</param>
        /// <param name="fileExtension">File extension.</param>
        /// <returns>Verification flag.</returns>
        public bool CheckFileExtension(FileType fileType, in string? fileExtension);
        
        /// <summary>
        /// Check if a file name is correct according to its context.
        /// </summary>
        /// <param name="fileContext">File context (purpose).</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Verification flag.</returns>
        /// 
        public bool CheckFileName(FileContext fileContext, in string? fileName);
        
        /// <summary>
        /// Check if a file is not empty (zero size).
        /// </summary>
        /// <param name="fileInfo">File info.</param>
        /// <returns>Verification flag.</returns>
        public bool CheckFileIsNotEmpty(ref FileInfo fileInfo);

    }
}
