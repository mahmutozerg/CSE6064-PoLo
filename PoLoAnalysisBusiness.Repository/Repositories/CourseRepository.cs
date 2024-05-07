using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class CourseRepository:GenericRepository<Course>,ICourseRepository
{
    private readonly DbSet<Course> _courses;

    public CourseRepository(AppDbContext context) : base(context)
    {
        _courses = context.Set<Course>();
    }

    public Task<List<Course>> GetActiveCoursesAsync(int page)
    {
        return _courses
            .Where(c=> !c.IsDeleted)
            .Skip(12*page)
            .Take(12)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Course>> GetAllCoursesAsync(int page)
    {
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