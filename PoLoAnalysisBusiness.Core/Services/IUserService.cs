using System.Security.Claims;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.DTO.Users;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.User;


namespace PoLoAnalysisBusiness.Core.Services;

public interface IUserService:IGenericService<AppUser>
{
    Task<CustomResponseDto<AppUser>> AddUserAsync(UserAddDto userAddDto, ClaimsIdentity claimsIdentity);
    Task<CustomResponseNoDataDto> DeleteUserAsync(UserDeleteDto userDeleteDto);
    
    Task<CustomResponseNoDataDto> AddUserToCoursesAsync(AddUsersToCoursesDto dto,string updatedBy);
    Task<CustomResponseNoDataDto> RemoveUserFromCourseAsync(RemoveUserFromCourseDto dto,string updatedBy);
    Task<CustomResponseDto<AppUser>> GetActiveUserWithCoursesByEMailAsync(string eMail);
    Task<CustomResponseDto<AppUser>> GetUserWithCoursesByEMailAsync(string eMail);
    Task<CustomResponseListDataDto<AppUser>> GetAllUsersByPageAsync(string page);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUsersByPageAsync(string page);
    Task<CustomResponseListDataDto<AppUser>> GetAllUsersWithCoursesByPageAsync(string page);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUsersWithCoursesByPageAsync(string page);
    


}