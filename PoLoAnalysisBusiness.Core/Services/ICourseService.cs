using SharedLibrary.DTOs.Responses;
using SharedLibrary.Models.business;


namespace PoLoAnalysisBusiness.Core.Services;

public interface ICourseService:IGenericService<Course>
{

    Task<CustomResponseListDataDto<Course>> GetActiveCoursesByNameByPageAsync(string name,string page);
    Task<CustomResponseListDataDto<Course>> GetAllCoursesByPageAsync(string page);
    Task<CustomResponseListDataDto<Course>> GetActiveCoursesByPageAsync(string page);


    Task<CustomResponseDto<Course>> GetCourseWithUploadedFilesWithResultFilesByIdAsync(string id);




}