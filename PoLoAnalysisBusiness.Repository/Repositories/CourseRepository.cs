using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class CourseRepository:GenericRepository<Course>,ICourseRepository
{
    private readonly DbSet<Course> _courses;
    private readonly int _activeCoursesMaxPage;
    private readonly int _allCoursesMaxPage;
    public CourseRepository(AppDbContext context) : base(context)
    {
        _courses = context.Set<Course>();
        _activeCoursesMaxPage = _courses.Count(c => !c.IsDeleted);
        _allCoursesMaxPage = _courses.Count();
    }

    public Task<List<Course>> GetActiveCoursesByNameByPageAsync(string name ,int page)
    {
        page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;
        return _courses
            .Where(c=> !c.IsDeleted && c.Id.ToLowerInvariant().Contains(name))
            .Skip(12*page)
            .Take(12)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Course>> GetActiveCoursesByPageAsync(int page)
    {
        page = page > _activeCoursesMaxPage ? _activeCoursesMaxPage : page;

        return _courses
            .Where(c=> !c.IsDeleted )
            .Skip(12*page)
            .Take(12)
            .AsNoTracking()
            .ToListAsync();
    }
    public Task<List<Course>> GetAllCoursesByPageAsync(int page)
    {
        page = page > _allCoursesMaxPage ? _allCoursesMaxPage : page;

        return _courses
            .Skip(12*page)
            .Take(12)
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