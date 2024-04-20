using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.DTO.Responses;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IResultService:IGenericService<Result>
{
    public void SetFilePath(string filePath, string id);
    public void AnalyzeExcel();

    public Task<CustomResponseDto<Result?>> AddAsync(string fileId, string path);

    public string GetResultPath();
    public Task<FileStreamResult> GetFileStream(string fileId);
}