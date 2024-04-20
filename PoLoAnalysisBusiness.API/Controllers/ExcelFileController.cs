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
    public async Task<IActionResult> UploadExcel( IFormFile model)
    {

        var result =await _appFileServices.WriteExcelFileToCurrentDirectoryAsync(model);
        
        //_poLoExcelServices.SetFilePath(result.Data.Path,result.Data.Id);
        //_poLoExcelServices.AnalyzeExcel();
        return CreateActionResult(result);
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(string id)
    {
        return await _appFileServices.GetFileStream(id);
    }
}