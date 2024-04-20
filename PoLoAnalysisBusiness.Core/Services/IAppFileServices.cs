using Microsoft.AspNetCore.Http;
using SharedLibrary.DTO;
using WebProgrammingTerm.Auth.Core.DTOs;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IAppFileServices
{
     Task<CustomResponseNoDataDto> WriteExcelFileToCurrentDirectoryAsync(IFormFile? model);
     bool IsExcelFile(IFormFile file);

}