using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class UserRepository:GenericRepository<AppUser>,IUserRepository
{
    private readonly DbSet<AppUser> _users;
    private readonly int _activeUsersMaxPage;
    private readonly int _allUsersMaxPage;
    private const int PageEntityCount = 12;
    public UserRepository(AppDbContext context) : base(context)
    {
        _users = context.Set<AppUser>();
        _activeUsersMaxPage = _users.Count(u => u.IsDeleted)/PageEntityCount;
        _allUsersMaxPage = _users.Count()/PageEntityCount;

    }

    public async Task<List<AppUser>> GetActiveUserWithCoursesByEMailAsync(string eMail)
    {
        return await _users
            .Where(u => u.EMail.Contains(eMail)  && !u.IsDeleted)
            .Include(u=> u.Courses)
            .ToListAsync();
    }
    public async Task<List<AppUser>> GetUserWithCoursesByEMilAsync(string eMail)
    {
        return await _users
            .Where(u => u.EMail.Contains(eMail))
            .Include(u=> u.Courses)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<List<AppUser>> GetAllUsersByPageAsync(int page)
    {
        //page = page > _allUsersMaxPage ? _allUsersMaxPage : page;
        
        return await _users
            .Skip(PageEntityCount * page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<List<AppUser>> GetActiveUsersByPageAsync(int page)
    {
        //page = page > _activeUsersMaxPage ? _activeUsersMaxPage : page;
        return await _users.Where(u=> !u.IsDeleted)
            .Skip(PageEntityCount * page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AppUser>> GetActiveUsersWithCourseByPageAsync(int page)
    {
        page = page > _activeUsersMaxPage ? _activeUsersMaxPage : page;

        
        return await _users
            .Where(u=> !u.IsDeleted)
            .Skip(PageEntityCount * page)
            .Take(PageEntityCount)
            .Include(u=> u.Courses)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AppUser>> GetAllUsersWithCourseByPageAsync(int page)
    {
        //page = page > _allUsersMaxPage ? _activeUsersMaxPage : page;

        return await _users
            .Skip(PageEntityCount * page)
            .Take(PageEntityCount)
            .Include(u=> u.Courses)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<AppUser?> GetUserWithCoursesByIdAsync(string id)
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

    public async Task<AppUser?> GetResultReadyCoursesAsync(string userId)
    {
        return await _users
            .Where(u => !u.IsDeleted && u.Id == userId)
            .Include(u => u.Courses.Where(c => !c.IsDeleted && c.File.Any(f => !f.IsDeleted)))
            .ThenInclude(course => course.File.Where(f => !f.IsDeleted))
            .ThenInclude(file => file.Result)
            .AsNoTracking()
            .SingleOrDefaultAsync();

    }



    public async Task<List<AppUser>> GetUserAsync(string eMail,int page)
    {
        //page = page > _allUsersMaxPage ? _allUsersMaxPage : page;

        return await _users
            .Where(u => u.EMail.Contains(eMail) )
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AppUser>> GetActiveUserAsync(string eMail)
    {
        return await _users
            .Where(u => !u.IsDeleted && u.EMail.Contains(eMail))
            .AsNoTracking()
            .ToListAsync();

    }

    public async Task<List<AppUser>> GetActiveUserWithCoursesByEMailByPageAsync(string eMail, int page)
    {
        page = page > _allUsersMaxPage ? _activeUsersMaxPage : page;

        return await _users
            .Where(u => u.EMail.Contains(eMail)  && !u.IsDeleted)
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .Include(u=> u.Courses)
            .ToListAsync();
        
        
    }

    public async Task<List<AppUser>> GetUserWithCoursesByEMailByPageAsync(string eMail, int page)
    {
        page = page > _allUsersMaxPage ? _activeUsersMaxPage : page;

        return await _users
            .Where(u => u.EMail.Contains(eMail) )
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .Include(u=> u.Courses)
            .ToListAsync();
    }
}