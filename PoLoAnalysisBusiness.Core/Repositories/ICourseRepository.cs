using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Core.Repositories;

public interface ICourseRepository:IGenericRepository<Course>
{
      Task<List<Course>> GetActiveCoursesAsync(int page);
      Task<List<Course>> GetAllCoursesAsync(int page);
      Task<Course?> GetCourseWithUploadedFilesWithResultFilesByIdAsync(string id);


}