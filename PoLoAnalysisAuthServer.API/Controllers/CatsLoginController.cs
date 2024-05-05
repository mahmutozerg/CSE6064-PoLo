using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisAuthServer.Core.Services;
using PoLoAnalysisAuthSever.Service.Services;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.Tokens;
using SharedLibrary.DTOs.User;
using StatusCodes = SharedLibrary.StatusCodes;

namespace PoLoAnalysisAuthServer.API.Controllers;

public class CatsLoginController:CustomControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    public CatsLoginController(IUserService userService, IAuthenticationService authentication)
    {
        _userService = userService;
        _authenticationService = authentication;
    }


    [HttpPost]
    public async Task<IActionResult> Login(CatsUserLogin catsUserLoginDto)
    {
        var userName = catsUserLoginDto.UserName.Split("@").First();
        var loginServices = new CatsLoginService(userName,catsUserLoginDto.Password);
        var result = loginServices.Start();
        
        var user = await _userService.GetUserByNameAsync(userName);

        if (user.StatusCode ==StatusCodes.NotFound)
        { 
            var userEmail = catsUserLoginDto.UserName + "@iku.edu.tr";
            
            var userResult =await _userService.CreateUserAsync(
                new UserCreateDto()
                {
                    Email = userEmail,
                    Password = Guid.NewGuid().ToString()
                });

            var userLoginDto = new UserLoginDto()
            {
                Email = userEmail,
                Password = "rand"
            };
           
           var tokenDto =await _authenticationService.CreateTokenAsync(userLoginDto);
           return CreateActionResult(tokenDto);
        
        }
        else
        {
            var userEmail = user.Data.Email;

            var userLoginDto = new UserLoginDto()
            {
                Email = userEmail,
                Password = "rand"
            };

            var tokenDto =await _authenticationService.CreateTokenAsync(userLoginDto);
            return CreateActionResult(tokenDto);

        }
        
        
    }
}