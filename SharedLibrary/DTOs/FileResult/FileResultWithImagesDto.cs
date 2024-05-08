using Microsoft.AspNetCore.Mvc;

namespace SharedLibrary.DTOs.FileResult;

public class FileResultWithImagesDto
{

    public FileStreamResult? ResultExcelFileStreamResult { get; set; }
    public List<FileStreamResult?> ResultDocxFileStreamResult { get; set; }
    public List<string> GraphResultPaths { get; set; } = new List<string>();
}