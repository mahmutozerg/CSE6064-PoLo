using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;

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
}