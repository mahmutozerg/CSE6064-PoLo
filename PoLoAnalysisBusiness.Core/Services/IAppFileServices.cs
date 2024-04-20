using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.DTO.Responses;
using File = PoLoAnalysisBusiness.Core.Models.File;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IAppFileServices:IGenericService<File>
{
     Task<CustomResponseDto<File>> WriteExcelFileToCurrentDirectoryAsync(IFormFile? model);
     bool IsExcelFile(IFormFile file);
     
     Task<FileStreamResult> GetFileStream(string filePath);

}