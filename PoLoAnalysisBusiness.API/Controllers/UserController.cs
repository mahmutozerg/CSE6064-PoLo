using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Services;
using SharedLibrary.DTOs;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisBusinessAPI.Controllers;

public class UserController:CustomControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize(Policy = "AdminBypassAuthServerPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddByIdAsync(UserAddDto userAddDto)
    {
 
        return CreateActionResult(await _userService.AddUserAsync(userAddDto,(ClaimsIdentity)User.Identity));
    }
    
    [Authorize(Policy = "AdminBypassAuthServerPolicy")]
    [HttpPost]
    public async Task<IActionResult> DeleteById(UserDeleteDto userDeleteDto)
    {
 
        return CreateActionResult(await _userService.DeleteUserAsync(userDeleteDto));
    }
}