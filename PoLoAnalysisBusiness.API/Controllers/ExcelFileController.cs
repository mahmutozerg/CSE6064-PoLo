using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;


namespace PoLoAnalysisBusinessAPI.Controllers;

public class ExcelFileController:CustomControllerBase
{
    private readonly IResultService _resultService;
    private readonly IAppFileServices _appFileServices;
    
    public ExcelFileController(IResultService resultService, IAppFileServices appFileServices)
    {
        _resultService = resultService;
        _appFileServices = appFileServices;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadExcel(IFormFile model)
    {
        
        var result =await _appFileServices.WriteExcelFileToCurrentDirectoryAsync(model);
        
        var fileResult = await _appFileServices.GetByIdAsync(result.Data.Id);
        
        _resultService.SetFilePath(fileResult.Data.Path,fileResult.Data.Id);
        _resultService.AnalyzeExcel();
        var filePath = _resultService.GetResultPath();
        var res = await _resultService.AddAsync(fileResult.Data.Id,filePath);
        return CreateActionResult(res);
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(string id)
    {
        return await _appFileServices.GetFileStream(id);
    }
}