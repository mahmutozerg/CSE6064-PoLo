using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IResultService:IGenericService<Result>
{
    public void SetFilePath(string filePath, string id);
    public void AnalyzeExcel();

    public Task<CustomResponseDto<Result?>> AddAsync(string fileId, string path);

    public Task<FileStreamResult> GetFileStreamAsync(string fileId);
}