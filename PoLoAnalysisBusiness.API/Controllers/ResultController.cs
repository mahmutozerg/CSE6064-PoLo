using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;
using SharedLibrary.DTOs;

namespace PoLoAnalysisBusinessAPI.Controllers;

[Authorize]
public class ResultController:CustomControllerBase
{
    private readonly IResultService _resultService;
    private readonly IAppFileServices _appFileServices;
    private readonly ICourseService _courseService;
    private readonly IUserService _userService;
    public ResultController(IResultService resultService, IAppFileServices appFileServices, ICourseService courseService, IUserService userService)
    {
        _resultService = resultService;
        _appFileServices = appFileServices;
        _courseService = courseService;
        _userService = userService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileByExcelId(string id)
    {
        var file = await _appFileServices.GetFileWithResultAsync(id);
        var fileStream = await _resultService.GetFileStreamAsync(file.Result.Id);
        
        return fileStream;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileByCourseId(string id)
    {
        var userId =  ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userWithCoursesAndFiles = (await _userService.GetUserWithCourseWithFilesWithResultByUserIdByCourseIdAsync(userId, id)).Data;

        return Ok();


    }
    [HttpPost]
    public async Task<IActionResult> CalculateResultByExcelId( ResultDto resultDto)
    {
        var userId =  ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var fileResult = await _appFileServices.GetByIdAsync(resultDto.ExcelFileId);
        _resultService.SetFilePath(fileResult.Data.Path,fileResult.Data.Id);
        _resultService.AnalyzeExcel();
        var result = await _resultService.AddAsync(fileResult.Data.Id,$"..\\UploadedFiles\\{fileResult.Data.Id}.xlsx",userId);
        
        return CreateActionResult(result);
    }
}