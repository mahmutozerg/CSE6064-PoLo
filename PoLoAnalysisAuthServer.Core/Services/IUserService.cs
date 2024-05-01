using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.Models;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisAuthServer.Core.Services;

public interface IUserService:IGenericService<User>
{
    Task<Response<User>> CreateUserAsync(UserCreateDto createUserDto);
    Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    Task<Response<User>> GetUserByEmailAsync(string eMail);
    Task<Response<NoDataDto>> Remove(string id);
    Task<Response<NoDataDto>> AddRoleToUser(string userEmail, string roleName);

    Task<CustomResponseListDataDto<User>> GetAllUsersByPage(string page);
    Task SendDeleteReqToBusinessAPI(User user);



}