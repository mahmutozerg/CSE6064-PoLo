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
}