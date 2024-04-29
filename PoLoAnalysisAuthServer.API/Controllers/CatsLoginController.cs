using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisAuthServer.Core.Services;
using PoLoAnalysisAuthSever.Service.Services;
using SharedLibrary.DTOs.User;
using StatusCodes = SharedLibrary.StatusCodes;

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
        if (user.StatusCode ==StatusCodes.NotFound)
        {
           await _userService.CreateUserAsync(
                new UserCreateDto()
                {
                    Email = userLoginDto.UserName + "@stu.iku.edu.tr",
                    Password = Guid.NewGuid().ToString()
                });
        }
        return CreateActionResult(result);
    }
}