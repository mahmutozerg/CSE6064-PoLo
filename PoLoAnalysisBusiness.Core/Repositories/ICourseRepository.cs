using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Core.Repositories;

public interface ICourseRepository:IGenericRepository<Course>
{
      Task<List<Course>> GetActiveCoursesByNameByPageAsync(string name ,int page);
      Task<List<Course>> GetAllCoursesByPageAsync(int page);
      Task<Course?> GetCourseWithUploadedFilesWithResultFilesByIdAsync(string id);
      Task<List<Course>> GetActiveCoursesByPageAsync(int page);


}