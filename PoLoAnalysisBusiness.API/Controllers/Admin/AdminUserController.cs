using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.DTO.Users;
using IUserService = PoLoAnalysisBusiness.Core.Services.IUserService;

namespace PoLoAnalysisBusinessAPI.Controllers.Admin;

[Authorize(Roles = "Admin")]
public class AdminUserController:CustomControllerBase
{
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;

    public AdminUserController(IUserService userService, ICourseService courseService)
    {
        _userService = userService;
        _courseService = courseService;
    }
    
    [HttpPut]
    public async Task<IActionResult> AddUserToCourses(AddUsersToCoursesDto dto)
    {

        var updatedBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return CreateActionResult(await _userService.AddUserToCourses(dto,updatedBy));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveUserFromCourse(RemoveUserFromCourseDto dto)
    {
        var updatedBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
         return CreateActionResult(await _userService.RemoveUserFromCourse(dto,updatedBy));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetActiveUserWithCourses(string eMail)
    {
        return CreateActionResult(await _userService.GetActiveUserWithCoursesByEMail(eMail));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserWithCourses(string eMail)
    {
        return CreateActionResult(await _userService.GetUserWithCoursesByEMail(eMail));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsersByPage(string page)
    {

        return CreateActionResult(await _userService.GetAllUsersByPage(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetActiveUsersByPage(string page)
    {

        return CreateActionResult(await _userService.GetActiveUsersByPage(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetActiveUsersWithCourseByPage(string page)
    {

        return CreateActionResult(await _userService.GetActiveUsersWithCoursesByPage(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsersWithCourseByPage(string page)
    {

        return CreateActionResult(await _userService.GetAllUsersWithCoursesByPage(page));
    }
}