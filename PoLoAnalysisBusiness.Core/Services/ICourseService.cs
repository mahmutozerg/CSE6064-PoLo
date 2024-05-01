using PoLoAnalysisBusiness.Core.Models;
using SharedLibrary.DTOs.Responses;


namespace PoLoAnalysisBusiness.Core.Services;

public interface ICourseService:IGenericService<Course>
{

    Task<CustomResponseListDataDto<Course>> GetActiveCoursesAsync();
    Task<CustomResponseListDataDto<Course>> GetAllCoursesAsync();
    
}