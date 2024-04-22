using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisAuthServer.Core.Services;
using PoLoAnalysisAuthSever.Service.Services;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthServer.API.Controllers;

public class CatsLoginController:CustomControllerBase
{
    private readonly IUserService _userService;

    public CatsLoginController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPost]
    public async Task<IActionResult> Login(CatsUserLogin userLoginDto)
    {
        var loginServices = new CatsLoginService(userLoginDto.UserName,userLoginDto.Password);
        var result = loginServices.Start();
        
        var user = await _userService.GetUserByNameAsync(userLoginDto.UserName);
        if (user.StatusCode == 404)
        {

           await _userService.CreateUserAsync(
                new UserCreateDto()
                {
                    Email = userLoginDto.UserName + "@stu.iku.edu.tr",
                    Password = userLoginDto.Password
                });
        }
        return CreateActionResult(result);
    }
}