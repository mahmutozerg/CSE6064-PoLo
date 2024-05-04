using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Users;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;


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


    public async Task<CustomResponseListDataDto<Course>> GetActiveCoursesAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if(res)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetActiveCoursesAsync(intPage),StatusCodes.Ok);

        throw new Exception(ResponseMessages.OutOfIndex);
    }

    public async Task<CustomResponseListDataDto<Course>> GetAllCoursesAsync(string page)
    {
        var res = int.TryParse(page, out var intPage);

        if(res)
            return CustomResponseListDataDto<Course>.Success(await _courseRepository.GetAllCoursesAsync(intPage),StatusCodes.Ok);
        throw new Exception(ResponseMessages.OutOfIndex);

    }
}