using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class CourseRepository:GenericRepository<Course>,ICourseRepository
{
    private readonly DbSet<Course> _courses;
    private readonly int _activeCoursesMaxPage;
    private readonly int _allCoursesMaxPage;
    private const int PageEntityCount = 12;

    public CourseRepository(AppDbContext context) : base(context)
    {
        _courses = context.Set<Course>();
        _activeCoursesMaxPage = _courses.Count(c => !c.IsDeleted)/PageEntityCount;
        _allCoursesMaxPage = _courses.Count()/PageEntityCount;
    }

    public Task<Course?> GetActiveCoursesWithFilesWithResultByNameAsync(string name)
    {
        return _courses
            .Where(c => !c.IsDeleted && c.Id == name)
            .Include(c=> c.File.Where(f=> !f.IsDeleted))
            .ThenInclude(f=> f.Result)
            .SingleOrDefaultAsync();
    }

    public Task<List<Course>> GetActiveCoursesByNameByPageAsync(string name ,int page)
    {
        //page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;
        return _courses
            .Where(c=> !c.IsDeleted && c.Id.ToLower().Contains(name.ToLower()))
            .Include(c=> c.Users.Where(u=> !u.IsDeleted))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Course>> GetActiveCoursesByPageAsync(int page)
    {
        //page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;

        return _courses
            .Where(c=> !c.IsDeleted )
            .Include(c=> c.Users.Where(u=> !u.IsDeleted))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Course>> GetAllCoursesByPageByNameAsync(string name, int page)
    {
        //page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;

        return _courses
            .Where(c=> !c.IsDeleted  && c.Id.ToLower().Contains(name.ToLower()))
            .Include(c=> c.Users.Where(u=> !u.IsDeleted && u.Courses.Exists(uc=> uc.Id == name && !uc.IsDeleted)))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
        
    }

    public Task<List<Course>> GetAllCompulsoryCoursesByPage(int page)
    {
        //page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;
        return _courses
            .Where(c => c.IsCompulsory)
            .Include(c=> c.Users.Where(u=> !u.IsDeleted))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Course>> GetActiveCompulsoryCoursesByPage(int page)
    {
        //page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;
        
        return _courses
            .Where(c => c.IsCompulsory && !c.IsDeleted)
            .Include(c=> c.Users.Where(u=> !u.IsDeleted))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
        
        
    }

    public Task<List<Course>> GetAllCompulsoryCoursesByPageByName(string name, int page)
    {
        //page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;
        
        return _courses
            .Where(c => c.IsCompulsory && c.Id.ToLower().Contains(name.ToLower()))
            .Include(c=> c.Users.Where(u=> !u.IsDeleted))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
        
    }

    public Task<List<Course>> GetActiveCompulsoryCoursesByPageByName(string name, int page)
    {
        //page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;
        
        return _courses
            .Where(c => c.IsCompulsory && !c.IsDeleted && c.Id.ToLower().Contains(name.ToLower()))
            .Include(c=> c.Users.Where(u=> !u.IsDeleted))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();    
    }

    public Task<List<Course>> GetAllCoursesByPageAsync(int page)
    {
        //page = page > _allCoursesMaxPage ? _allCoursesMaxPage : page;

        return _courses
            .Include(c=> c.Users.Where(u=> !u.IsDeleted))
            .Skip(PageEntityCount*page)
            .Take(PageEntityCount)
            .AsNoTracking()
            .ToListAsync();
        
    }

    public Task<Course?> GetCourseWithUploadedFilesWithResultFilesByIdAsync(string id)
    {
        return _courses
            .Where(c => !c.IsDeleted && c.Id== id)
            .Include(c=> c.File.Where(f=> !f.IsDeleted))
            .ThenInclude(f=> f.Result)
            .SingleOrDefaultAsync();
    }


}