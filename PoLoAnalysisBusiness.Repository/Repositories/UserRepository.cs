using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class UserRepository:GenericRepository<AppUser>,IUserRepository
{
    private readonly DbSet<AppUser> _users;
    public UserRepository(AppDbContext context) : base(context)
    {
        _users = context.Set<AppUser>();
    }

    public async Task<AppUser?> GetActiveUserWithCoursesByEmailAsync(string eMail)
    {
        return await _users
            .Where(u => u.EMail == eMail && !u.IsDeleted)
            .Include(u=> u.Courses)
            .FirstOrDefaultAsync();
    }
    public async Task<AppUser?> GetUserWithCoursesByEmailAsync(string eMail)
    {
        return await _users
            .Where(u => u.EMail == eMail)
            .Include(u=> u.Courses)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    public async Task<List<AppUser>> GetAllUsersByPage(int page)
    {
        return await _users
            .Skip(12 * page)
            .Take(12)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<List<AppUser>> GetActiveUsersByPage(int page)
    {
        return await _users.Where(u=> !u.IsDeleted)
            .Skip(12 * page)
            .Take(12)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AppUser>> GetActiveUsersWithCourseByPage(int page)
    {
        return await _users
            .Where(u=> !u.IsDeleted)
            .Include(u=> u.Courses)
            .Skip(12 * page)
            .Take(12)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AppUser>> GetAllUsersWithCourseByPage(int page)
    {
        return await _users
            .Include(u=> u.Courses)
            .Skip(12 * page)
            .Take(12)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<AppUser?> GetUserWithCoursesById(string id)
    {
        return await _users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Courses.Where(c => !c.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<AppUser?> GetUserWithCourseWithFilesWithResultByUserIdByCourseIdAsync(string userId, string courseId)
    {
        return await _users
            .Where(u => !u.IsDeleted && u.Id == userId)
            .Include(u => u.Courses.Where(c => !c.IsDeleted && c.Id == courseId))
            .ThenInclude(course => course.File.Where(f => !f.IsDeleted))
            .ThenInclude(file => file.Result)
            .SingleOrDefaultAsync();
    }
}