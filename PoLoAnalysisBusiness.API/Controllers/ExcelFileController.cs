using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;


namespace PoLoAnalysisBusinessAPI.Controllers;
[Authorize]
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
        var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var cId = WebUtility.UrlDecode(courseId);

        var course = await _courseService.GetByIdAsync( WebUtility.UrlDecode(courseId));

        var result =await _appFileServices.AddFileAsync(file,cId,userId);
        
        var fileResult = await _appFileServices.GetByIdAsync(result.Data.Id);

        try
        {
            _resultService.SetFilePath(fileResult.Data.Path,fileResult.Data.Id);
            await _resultService.AnalyzeExcel(course.Data);
        }
        catch (Exception e)
        {
            
            await _appFileServices.RemoveAsync(result.Data.Id,"System");
            throw;
        }

        var res = await _resultService.AddAsync(fileResult.Data.Id,$"..\\UploadedFiles\\{fileResult.Data.Id}",userId);
        return CreateActionResult(res);
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(string id)
    {
        return await _appFileServices.GetFileStreamAsync(id);
    }

    
 
}