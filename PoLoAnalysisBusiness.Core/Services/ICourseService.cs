using SharedLibrary.DTOs.Responses;
using SharedLibrary.Models.business;


namespace PoLoAnalysisBusiness.Core.Services;

public interface ICourseService:IGenericService<Course>
{

    Task<CustomResponseNoDataDto> DeleteCourseWithFilesWithResultByIdAsync(string id,string updatedBy);
    Task<CustomResponseListDataDto<Course>> GetActiveCoursesByNameByPageAsync(string name,string page);
    Task<CustomResponseListDataDto<Course>> GetAllCoursesByPageAsync(string page);
    Task<CustomResponseListDataDto<Course>> GetActiveCoursesByPageAsync(string page);


    Task<CustomResponseDto<Course>> GetCourseWithUploadedFilesWithResultFilesByIdAsync(string id);


    Task<CustomResponseListDataDto<Course>> GetAllCoursesByPageByNameAsync(string name, string page);
    Task<CustomResponseListDataDto<Course>> GetAllCompulsoryCoursesByPage(string page);
    Task<CustomResponseListDataDto<Course>> GetActiveCompulsoryCoursesByPage(string page);
    Task<CustomResponseListDataDto<Course>> GetAllCompulsoryCoursesByPageByName(string name, string page);
    Task<CustomResponseListDataDto<Course>> GetActiveCompulsoryCoursesByPageByName(string name, string page);
}