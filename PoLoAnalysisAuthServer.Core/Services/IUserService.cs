using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.Models;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthServer.Core.Services;

public interface IUserService
{
    Task<Response<User>> CreateUserAsync(UserCreateDto createUserDto);
    Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    Task<Response<NoDataDto>> Remove(string id);
    Task<Response<NoDataDto>> AddRoleToUser(string userEmail, string roleName);

}