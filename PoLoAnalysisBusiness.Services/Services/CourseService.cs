using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.Models.business;


namespace PoLoAnalysisBusiness.Services.Services;

public class CourseService:GenericService<Course>,ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CourseService(ICourseRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    {
        _courseRepository = repository;
        _unitOfWork = unitOfWork;
    }


    public async Task<CustomResponseNoDataDto> DeleteCourseWithFilesWithResultByIdAsync(string id, string updatedBy)
    {
        var course = await _courseRepository.GetActiveCoursesWithFilesWithResultByNameAsync(id);
        ArgumentNullException.ThrowIfNull(course);
        course.IsDeleted = true;
        foreach (var files in course.File)
        {
            files.IsDeleted = true;
            if (files.Result is not null)
                files.Result.IsDeleted = true;
        }

        await UpdateAsync(course, updatedBy);
        await _unitOfWork.CommitAsync();

        return CustomResponseNoDataDto.Success(StatusCodes.Ok);

    }

    public async Task<CustomResponseListDataDto<Course>> GetActiveCoursesByNameByPageAsync(string name ,string page)
    {
        var res = int.TryParse(page, out var intPage);
        
        if(res)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetActiveCoursesByNameByPageAsync(name,intPage),StatusCodes.Ok);

        throw new Exception(ResponseMessages.OutOfIndex);
    }



    public async Task<CustomResponseListDataDto<Course>> GetAllCoursesByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        
        if(res)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetAllCoursesByPageAsync(intPage),StatusCodes.Ok);
        throw new Exception(ResponseMessages.OutOfIndex);

    }

    public async Task<CustomResponseListDataDto<Course>> GetActiveCoursesByPageAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);

        if(res)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetActiveCoursesByPageAsync(intPage),StatusCodes.Ok);
        throw new Exception(ResponseMessages.OutOfIndex);    
    }

    public async Task<CustomResponseDto<Course>> GetCourseWithUploadedFilesWithResultFilesByIdAsync(string id)
    {
        var course = await _courseRepository.GetCourseWithUploadedFilesWithResultFilesByIdAsync(id);
        ArgumentNullException.ThrowIfNull(course);
        
        return CustomResponseDto<Course>.Success(course, StatusCodes.Ok);
    }

    public async Task<CustomResponseListDataDto<Course>> GetAllCoursesByPageByNameAsync(string name, string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >=0)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetAllCoursesByPageByNameAsync(name, intPage), StatusCodes.Ok);
        
        throw new Exception(ResponseMessages.OutOfIndex);    
    }

    public async Task<CustomResponseListDataDto<Course>> GetAllCompulsoryCoursesByPage(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >=0)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetAllCompulsoryCoursesByPage(intPage), StatusCodes.Ok);
        
        throw new Exception(ResponseMessages.OutOfIndex);  
    }

    public async Task<CustomResponseListDataDto<Course>> GetActiveCompulsoryCoursesByPage(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >=0)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetActiveCompulsoryCoursesByPage(intPage), StatusCodes.Ok);
        
        throw new Exception(ResponseMessages.OutOfIndex);  
        
    }

    public async Task<CustomResponseListDataDto<Course>> GetAllCompulsoryCoursesByPageByName(string name, string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >=0)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetAllCompulsoryCoursesByPageByName(name, intPage), StatusCodes.Ok);
        
        throw new Exception(ResponseMessages.OutOfIndex);   
        
        
    }

    public async Task<CustomResponseListDataDto<Course>> GetActiveCompulsoryCoursesByPageByName(string name, string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (res && intPage >=0)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetActiveCompulsoryCoursesByPageByName(name, intPage), StatusCodes.Ok);
        
        throw new Exception(ResponseMessages.OutOfIndex);       }
}