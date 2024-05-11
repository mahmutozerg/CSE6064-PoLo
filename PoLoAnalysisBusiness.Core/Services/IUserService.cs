using System.Security.Claims;
using PoLoAnalysisBusiness.DTO.Users;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.DTOs.User;
using SharedLibrary.Models.business;


namespace PoLoAnalysisBusiness.Core.Services;

public interface IUserService:IGenericService<AppUser>
{
    Task<CustomResponseDto<AppUser>> AddUserAsync(UserAddDto userAddDto, ClaimsIdentity claimsIdentity);
    Task<CustomResponseNoDataDto> DeleteUserAsync(UserDeleteDto userDeleteDto);
    
    Task<CustomResponseNoDataDto> AddUserToCoursesAsync(AddUsersToCoursesDto dto,string updatedBy);
    Task<CustomResponseNoDataDto> RemoveUserFromCourseAsync(RemoveUserFromCourseDto dto,string updatedBy);
    Task<CustomResponseDto<List<AppUser>>> GetActiveUserWithCoursesByEMailAsync(string eMail);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUserWithCoursesByEMailByPageAsync(string eMail,string page);
    Task<CustomResponseDto<List<AppUser>>> GetUserAsync(string eMail, string page);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUserAsync(string eMail,string page);
    Task<CustomResponseListDataDto<AppUser>> GetAllUsersByPageAsync(string page);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUsersByPageAsync(string page);
    Task<CustomResponseListDataDto<AppUser>> GetAllUsersWithCoursesByPageAsync(string page);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUsersWithCoursesByPageAsync(string page);
    
    Task<CustomResponseDto<AppUser>> GetUserWithCoursesByIdAsync(string id);

    Task<CustomResponseDto<AppUser>> GetUserWithCourseWithFilesWithResultByUserIdByCourseIdAsync(string userId, string courseId);

    Task<CustomResponseDto<AppUser>> GetReadyResultCoursesAsync(string userId);


    Task<CustomResponseListDataDto<AppUser>> GetUserWithCoursesByEMailByPageAsync(string eMail, string page);
}