using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;


namespace PoLoAnalysisBusinessAPI.Controllers;

public class ExcelFileController:CustomControllerBase
{
    private readonly IResultService _resultService;
    private readonly IAppFileServices _appFileServices;
    private readonly ICourseService _courseService;
    
    public ExcelFileController(IResultService resultService, IAppFileServices appFileServices, ICourseService courseService)
    {
        _resultService = resultService;
        _appFileServices = appFileServices;
        _courseService = courseService;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadExcel([FromForm]IFormFile file,[FromForm]string courseId)
    {
        // TODO Kullanıcı dosya yüklediyse override etmeyi yaz

        var course = await _courseService.GetByIdAsync(courseId);
        
        var result =await _appFileServices.AddFileAsync(file,courseId);
        
        var fileResult = await _appFileServices.GetByIdAsync(result.Data.Id);
        
        _resultService.SetFilePath(fileResult.Data.Path,fileResult.Data.Id);
        _resultService.AnalyzeExcel();
        var res = await _resultService.AddAsync(fileResult.Data.Id,$"..\\UploadedFiles\\{fileResult.Data.Id}.xlsx");
        return CreateActionResult(res);
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(string id)
    {
        return await _appFileServices.GetFileStreamAsync(id);
    }
}