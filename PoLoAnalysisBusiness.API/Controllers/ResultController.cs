﻿using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetFile(string id)
    {
        var fileStream = await _resultService.GetFileStream(id);
        
        return fileStream;
    }
    
    [HttpPost]
    public async Task<IActionResult> CalculateResultByExcelId( ResultDto resultDto)
    {

        var fileResult = await _appFileServices.GetById(resultDto.ExcelFileId);
        _resultService.SetFilePath(fileResult.Data.Path,fileResult.Data.Id);
        _resultService.AnalyzeExcel();
        var filePath = _resultService.GetResultPath();
        var result = await _resultService.AddAsync(fileResult.Data.Id,filePath);
        
        return CreateActionResult(result);
    }
}