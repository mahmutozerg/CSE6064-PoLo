using SharedLibrary.DTOs.Responses;
using SharedLibrary.Models.business;


namespace PoLoAnalysisBusiness.Core.Services;

public interface ICourseService:IGenericService<Course>
{

    Task<CustomResponseListDataDto<Course>> GetActiveCoursesAsync(string page);
    Task<CustomResponseListDataDto<Course>> GetAllCoursesAsync(string page);


}