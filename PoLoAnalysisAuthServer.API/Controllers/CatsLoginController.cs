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
    private readonly ITokenService _tokenService;
    public CatsLoginController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }


    [HttpPost]
    public async Task<IActionResult> Login(CatsUserLogin userLoginDto)
    {
        var loginServices = new CatsLoginService(userLoginDto.UserName.Split("@").First(),userLoginDto.Password);
        var result = loginServices.Start();
        
        var user = await _userService.GetUserByNameAsync(userLoginDto.UserName);
        if (user.StatusCode ==StatusCodes.NotFound)
        {
           var userResult =await _userService.CreateUserAsync(
                new UserCreateDto()
                {
                    Email = userLoginDto.UserName + "@iku.edu.tr",
                    Password = Guid.NewGuid().ToString()
                });

           var tokenDto =await _tokenService.CreateTokenAsync(userResult.Data);
           return CreateActionResult(CustomResponseDto<TokenDto>.Success(tokenDto,StatusCodes.Ok));

        }
        else
        {
            var tokenDto =await _tokenService.CreateTokenAsync(user.Data);
            return CreateActionResult(CustomResponseDto<TokenDto>.Success(tokenDto,StatusCodes.Ok));

        }
        
        
    }
}