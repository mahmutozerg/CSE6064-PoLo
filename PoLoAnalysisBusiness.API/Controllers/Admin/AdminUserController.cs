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

    public AdminUserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPut]
    public async Task<IActionResult> AddUserToCourses(AddUsersToCoursesDto dto)
    {

        var updatedBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return CreateActionResult(await _userService.AddUserToCoursesAsync(dto,updatedBy));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveUserFromCourse(RemoveUserFromCourseDto dto)
    {
        var updatedBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
         return CreateActionResult(await _userService.RemoveUserFromCourseAsync(dto,updatedBy));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetActiveUserWithCourses(string eMail)
    {
        return CreateActionResult(await _userService.GetActiveUserWithCoursesByEMailAsync(eMail));
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetUserWithCourses(string eMail)
    {
        return CreateActionResult(await _userService.GetUserWithCoursesByEMailAsync(eMail));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUser(string eMail)
    {
        return CreateActionResult(await _userService.GetUserAsync(eMail));
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveUser(string eMail)
    {
        return CreateActionResult(await _userService.GetActiveUserAsync(eMail));
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsersByPage(string page)
    {

        return CreateActionResult(await _userService.GetAllUsersByPageAsync(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetActiveUsersByPage(string page)
    {

        return CreateActionResult(await _userService.GetActiveUsersByPageAsync(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetActiveUsersWithCourseByPage(string page)
    {

        return CreateActionResult(await _userService.GetActiveUsersWithCoursesByPageAsync(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsersWithCourseByPage(string page)
    {

        return CreateActionResult(await _userService.GetAllUsersWithCoursesByPageAsync(page));
    }
}