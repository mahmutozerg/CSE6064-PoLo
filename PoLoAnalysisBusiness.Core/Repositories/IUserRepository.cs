using PoLoAnalysisBusiness.Core.Models;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Core.Repositories;

public interface IUserRepository:IGenericRepository<AppUser>
{
    Task<AppUser?> GetActiveUserWithCoursesByEmailAsync(string eMail);
    Task<AppUser?> GetUserWithCoursesByEmailAsync(string eMail);
    Task<List<AppUser>> GetAllUsersByPage(int page);
    Task<List<AppUser>> GetActiveUsersByPage(int page);

    Task<List<AppUser>> GetActiveUsersWithCourseByPage(int page);
    Task<List<AppUser>> GetAllUsersWithCourseByPage(int page);

}