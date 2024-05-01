using PoLoAnalysisBusiness.Core.Models;

namespace PoLoAnalysisBusiness.Core.Repositories;

public interface ICourseRepository:IGenericRepository<Course>
{
      Task<List<Course>> GetActiveCoursesAsync();
      Task<List<Course>> GetAllCoursesAsync();

}