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
    
    Task<CustomResponseNoDataDto> AddUserToCourses(AddUsersToCoursesDto dto,string updatedBy);
    Task<CustomResponseNoDataDto> RemoveUserFromCourse(RemoveUserFromCourseDto dto,string updatedBy);
    Task<CustomResponseDto<AppUser>> GetActiveUserWithCoursesByEMail(string eMail);
    Task<CustomResponseDto<AppUser>> GetUserWithCoursesByEMail(string eMail);
    Task<CustomResponseListDataDto<AppUser>> GetAllUsersByPage(string page);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUsersByPage(string page);
    Task<CustomResponseListDataDto<AppUser>> GetAllUsersWithCoursesByPage(string page);
    Task<CustomResponseListDataDto<AppUser>> GetActiveUsersWithCoursesByPage(string page);
    


}