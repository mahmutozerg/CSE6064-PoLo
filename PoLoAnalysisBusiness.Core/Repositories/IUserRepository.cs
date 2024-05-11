using SharedLibrary.Models;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Core.Repositories;

public interface IUserRepository:IGenericRepository<AppUser>
{
    Task<List<AppUser>> GetActiveUserWithCoursesByEmailAsync(string eMail);
    Task<List<AppUser>> GetUserWithCoursesByEmailAsync(string eMail);
    Task<List<AppUser>> GetAllUsersByPageAsync(int page);
    Task<List<AppUser>> GetActiveUsersByPageAsync(int page);

    Task<List<AppUser>> GetActiveUsersWithCourseByPageAsync(int page);
    Task<List<AppUser>> GetAllUsersWithCourseByPageAsync(int page);

    Task<AppUser?> GetUserWithCoursesByIdAsync(string id);

    Task<AppUser?> GetUserWithCourseWithFilesWithResultByUserIdByCourseIdAsync(string userId, string courseId);

    Task<AppUser?> GetResultReadyCoursesAsync(string userId);
    Task<List<AppUser>> GetUserAsync(string eMail);
    Task<List<AppUser>> GetActiveUserAsync(string eMail);
}