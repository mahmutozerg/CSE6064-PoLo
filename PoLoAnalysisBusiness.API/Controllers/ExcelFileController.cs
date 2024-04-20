using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;


namespace PoLoAnalysisBusinessAPI.Controllers;

public class ExcelFileController:CustomControllerBase
{
    private readonly IPoLoExcelServices _poLoExcelServices;
    private readonly IAppFileServices _appFileServices;
    
    public ExcelFileController(IPoLoExcelServices poLoExcelServices, IAppFileServices appFileServices)
    {
        _poLoExcelServices = poLoExcelServices;
        _appFileServices = appFileServices;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadExcel( IFormFile model)
    {

        var result =await _appFileServices.WriteExcelFileToCurrentDirectoryAsync(model);
        _poLoExcelServices.SetFilePath(result.Data.Path,result.Data.Id);
        _poLoExcelServices.AnalyzeExcel();
        return CreateActionResult(result);
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(string id)
    {

        return await _appFileServices.GetFileStream(id);

       
    }
}