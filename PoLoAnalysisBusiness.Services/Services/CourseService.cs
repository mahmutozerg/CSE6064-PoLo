using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Users;
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


}