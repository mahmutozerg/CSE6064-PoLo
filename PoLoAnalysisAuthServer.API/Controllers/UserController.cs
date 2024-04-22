using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisAuthServer.Core.Services;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthServer.API.Controllers;

public class UserController:CustomControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
     public UserController(IUserService userService, IGenericService<User> genericService, IAuthenticationService authenticationService)
     {
         _userService = userService;
         _authenticationService = authenticationService;
     }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateDto createUserDto)
    {
        var result = await _userService.CreateUserAsync(createUserDto);
        if (result.StatusCode != 200) 
            return CreateActionResult(result);
        var loginD = new UserLoginDto()
        {
            Email = createUserDto.Email,
            Password = createUserDto.Password
        };
        var token = await _authenticationService.CreateTokenAsync(loginD);
        return CreateActionResult(  token);

    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var a = User.Identity;
        var user = await _userService.GetUserByNameAsync(User.Identity.Name);

        return CreateActionResult(user);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        var user = await _userService.Remove(User.FindFirstValue(ClaimTypes.NameIdentifier));

        return CreateActionResult(user);
    }
    


}