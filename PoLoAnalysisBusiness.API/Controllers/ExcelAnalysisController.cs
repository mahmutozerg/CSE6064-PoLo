using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;


namespace PoLoAnalysisBusinessAPI.Controllers;

public class ExcelAnalysisController:CustomControllerBase
{
    private readonly IPoLoExcelServices _poLoExcelServices;
    private readonly IAppFileServices _appFileServices;
    
    public ExcelAnalysisController(IPoLoExcelServices poLoExcelServices, IAppFileServices appFileServices)
    {
        _poLoExcelServices = poLoExcelServices;
        _appFileServices = appFileServices;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTokenByClient( IFormFile model)
    {

        var result =await _appFileServices.WriteExcelFileToCurrentDirectoryAsync(model);
        return CreateActionResult(result);
    }
}