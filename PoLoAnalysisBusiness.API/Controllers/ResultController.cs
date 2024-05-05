using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using SharedLibrary.DTOs;

namespace PoLoAnalysisBusinessAPI.Controllers;

public class ResultController:CustomControllerBase
{
    private readonly IResultService _resultService;
    private readonly IAppFileServices _appFileServices;

    public ResultController(IResultService resultService, IAppFileServices appFileServices)
    {
        _resultService = resultService;
        _appFileServices = appFileServices;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileByExcelId(string id)
    {
        var file = await _appFileServices.GetFileWithResultAsync(id);
        var fileStream = await _resultService.GetFileStreamAsync(file.Result.Id);
        
        return fileStream;
    }
    
    [HttpPost]
    public async Task<IActionResult> CalculateResultByExcelId( ResultDto resultDto)
    {

        var fileResult = await _appFileServices.GetByIdAsync(resultDto.ExcelFileId);
        _resultService.SetFilePath(fileResult.Data.Path,fileResult.Data.Id);
        _resultService.AnalyzeExcel();
        var result = await _resultService.AddAsync(fileResult.Data.Id,$"..\\UploadedFiles\\{fileResult.Data.Id}.xlsx");
        
        return CreateActionResult(result);
    }
}