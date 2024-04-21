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
    
    [HttpPost]
    public async Task<IActionResult> CreateUser( UserCreateDto userCreateDto)
    {
        throw new NotImplementedException();
    }
}