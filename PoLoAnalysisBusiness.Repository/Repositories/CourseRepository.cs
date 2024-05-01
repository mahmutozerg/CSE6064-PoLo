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

    public Task<List<Course>> GetActiveCoursesAsync()
    {
        return _courses
            .Where(c=> !c.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Course>> GetAllCoursesAsync()
    {
        return _courses
            .AsNoTracking()
            .ToListAsync();    }
}