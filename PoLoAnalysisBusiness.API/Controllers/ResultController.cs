using System.IO.Compression;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;
using SharedLibrary.DTOs;
using SharedLibrary.DTOs.FileResult;

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
    public async Task<IActionResult> GetFileByCourseId(string id)
    {
        var userId =  ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userWithCoursesAndFiles = (await _userService.GetUserWithCourseWithFilesWithResultByUserIdByCourseIdAsync(userId, id)).Data;

        var zipFileByteArray = await (_resultService
            .GetFileStreamAsync(userWithCoursesAndFiles
                .Courses
                .SingleOrDefault(c => c.Id == id)
                .File
                .First()
                .Result.Id));

        return File(zipFileByteArray,"application/zip","archive.zip");

    }

}