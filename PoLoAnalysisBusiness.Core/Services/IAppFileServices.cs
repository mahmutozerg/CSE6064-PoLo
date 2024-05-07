using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary.DTOs.Responses;
using File = SharedLibrary.Models.business.File;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IAppFileServices:IGenericService<File>
{
     Task<CustomResponseDto<File>> AddFileAsync(IFormFile? model , string courseName,string createdBy);
     bool IsExcelFile(IFormFile file);
     
     Task<FileStreamResult> GetFileStreamAsync(string filePath);
     Task<File> GetFileWithResultAsync(string id);


}