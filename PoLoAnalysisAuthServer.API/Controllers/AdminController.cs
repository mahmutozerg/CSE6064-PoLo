using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisAuthServer.Core.Services;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.User;
using StatusCodes = SharedLibrary.StatusCodes;

namespace PoLoAnalysisAuthServer.API.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController:CustomControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public AdminController(IUserService userService, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUserToRole(UserRoleDto userRoleDto)
    {
        var user = await _userService.AddRoleToUser(userRoleDto.UserMail, userRoleDto.RoleName);
        var userRefreshToken = await _authenticationService.GetUserRefreshTokenByEmail(userRoleDto.UserMail);
        var userAccessToken = await _authenticationService.CreateTokenByRefreshToken(userRefreshToken.Token);

        return CreateActionResult(userAccessToken);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetUsers(string page)
    {

        return CreateActionResult(await _userService.GetAllUsersByPage(page));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserByEmail(string eMail)
    {
        var user = await _userService.GetUserByEmailAsync(eMail);

        if (user.StatusCode != StatusCodes.Ok)
            return CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.NotFound,
                ResponseMessages.UserNotFound));
        
        await _userService.SendDeleteReqToBusinessApi(user.Data);
        return CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Ok));



    }
    


}